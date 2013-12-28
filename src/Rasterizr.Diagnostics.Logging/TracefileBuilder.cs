using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Pipeline;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Util;
using SlimShader;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Diagnostics.Logging
{
    public class TracefileBuilder
    {
        private readonly Tracefile _tracefile;
        private TracefileFrame _currentFrame;
        private bool _writtenBeginFrame;
        private int _frameNumber;
        private int _operationNumber;

        public Tracefile Tracefile
        {
            get { return _tracefile; }
        }

        public TracefileBuilder(
            Device device, 
            int? renderTargetViewID = null, int? renderTargetArrayIndex = null,
            int? pixelX = null, int? pixelY = null)
        {
            _tracefile = new Tracefile();

            Func<OperationType, DiagnosticEventHandler> makeHandler = type =>
                (sender, e) => LogOperation(type, e.Arguments);

            device.CreatingBlendState += makeHandler(OperationType.DeviceCreateBlendState);
            device.CreatingBuffer += makeHandler(OperationType.DeviceCreateBuffer);
            device.CreatingDepthStencilState += makeHandler(OperationType.DeviceCreateDepthStencilState);
            device.CreatingDepthStencilView += makeHandler(OperationType.DeviceCreateDepthStencilView);
            device.CreatingGeometryShader += makeHandler(OperationType.DeviceCreateGeometryShader);
            device.CreatingInputLayout += makeHandler(OperationType.DeviceCreateInputLayout);
            device.CreatingPixelShader += makeHandler(OperationType.DeviceCreatePixelShader);
            device.CreatingRasterizerState += makeHandler(OperationType.DeviceCreateRasterizerState);
            device.CreatingRenderTargetView += makeHandler(OperationType.DeviceCreateRenderTargetView);
            device.CreatingSamplerState += makeHandler(OperationType.DeviceCreateSamplerState);
            device.CreatingShaderResourceView += makeHandler(OperationType.DeviceCreateShaderResourceView);
            device.CreatingSwapChain += makeHandler(OperationType.DeviceCreateSwapChain);
            device.CreatingTexture1D += makeHandler(OperationType.DeviceCreateTexture1D);
            device.CreatingTexture2D += makeHandler(OperationType.DeviceCreateTexture2D);
            device.CreatingTexture3D += makeHandler(OperationType.DeviceCreateTexture3D);
            device.CreatingVertexShader += makeHandler(OperationType.DeviceCreateVertexShader);

            var deviceContext = device.ImmediateContext;
            deviceContext.ClearingDepthStencilView += makeHandler(OperationType.DeviceContextClearDepthStencilView);
            deviceContext.ClearingRenderTargetView += makeHandler(OperationType.DeviceContextClearRenderTargetView);
            deviceContext.Drawing += makeHandler(OperationType.DeviceContextDraw);
            deviceContext.DrawingIndexed += makeHandler(OperationType.DeviceContextDrawIndexed);
            deviceContext.DrawingInstanced += makeHandler(OperationType.DeviceContextDrawInstanced);
            deviceContext.GeneratingMips += makeHandler(OperationType.DeviceContextGenerateMips);
            deviceContext.Presenting += makeHandler(OperationType.DeviceContextPresent);
            deviceContext.SettingBufferData += makeHandler(OperationType.DeviceContextSetBufferData);
            deviceContext.SettingTextureData += makeHandler(OperationType.DeviceContextSetTextureData);

            var inputAssemblerStage = deviceContext.InputAssembler;
            inputAssemblerStage.SettingInputLayout += makeHandler(OperationType.InputAssemblerStageSetInputLayout);
            inputAssemblerStage.SettingPrimitiveTopology += makeHandler(OperationType.InputAssemblerStageSetPrimitiveTopology);
            inputAssemblerStage.SettingVertexBuffers += makeHandler(OperationType.InputAssemblerStageSetVertexBuffers);
            inputAssemblerStage.SettingIndexBuffer += makeHandler(OperationType.InputAssemblerStageSetIndexBuffer);

            inputAssemblerStage.ProcessedVertex += (sender, e) =>
            {
                var lastEvent = _currentFrame.Events.Last();
                lastEvent.InputAssemblerOutputs.Add(e.Vertex);
            };

            var vertexShaderStage = deviceContext.VertexShader;
            vertexShaderStage.SettingShader += makeHandler(OperationType.VertexShaderStageSetShader);
            vertexShaderStage.SettingConstantBuffers += makeHandler(OperationType.VertexShaderStageSetConstantBuffers);
            vertexShaderStage.SettingSamplers += makeHandler(OperationType.VertexShaderStageSetSamplers);
            vertexShaderStage.SettingShaderResources += makeHandler(OperationType.VertexShaderStageSetShaderResources);

            var geometryShaderStage = deviceContext.GeometryShader;
            geometryShaderStage.SettingShader += makeHandler(OperationType.GeometryShaderStageSetShader);
            geometryShaderStage.SettingConstantBuffers += makeHandler(OperationType.GeometryShaderStageSetConstantBuffers);
            geometryShaderStage.SettingSamplers += makeHandler(OperationType.GeometryShaderStageSetSamplers);
            geometryShaderStage.SettingShaderResources += makeHandler(OperationType.GeometryShaderStageSetShaderResources);

            var rasterizerStage = deviceContext.Rasterizer;
            rasterizerStage.SettingState += makeHandler(OperationType.RasterizerStageSetState);
            rasterizerStage.SettingViewports += makeHandler(OperationType.RasterizerStageSetViewports);

            var pixelShaderStage = deviceContext.PixelShader;
            pixelShaderStage.SettingShader += makeHandler(OperationType.PixelShaderStageSetShader);
            pixelShaderStage.SettingConstantBuffers += makeHandler(OperationType.PixelShaderStageSetConstantBuffers);
            pixelShaderStage.SettingSamplers += makeHandler(OperationType.PixelShaderStageSetSamplers);
            pixelShaderStage.SettingShaderResources += makeHandler(OperationType.PixelShaderStageSetShaderResources);

            var outputMergerStage = deviceContext.OutputMerger;
            outputMergerStage.SettingDepthStencilState += makeHandler(OperationType.OutputMergerStageSetDepthStencilState);
            outputMergerStage.SettingDepthStencilReference += makeHandler(OperationType.OutputMergerStageSetDepthStencilReference);
            outputMergerStage.SettingBlendState += makeHandler(OperationType.OutputMergerStageSetBlendState);
            outputMergerStage.SettingBlendFactor += makeHandler(OperationType.OutputMergerStageSetBlendFactor);
            outputMergerStage.SettingBlendSampleMask += makeHandler(OperationType.OutputMergerStageSetBlendSampleMask);
            outputMergerStage.SettingTargets += makeHandler(OperationType.OutputMergerStageSetTargets);

            if (pixelX != null && pixelY != null)
            {
                deviceContext.ClearingRenderTargetView += (sender, e) =>
                {
                    if ((int) e.Arguments[0] != renderTargetViewID)
                        return;
                    var color = (Color4) e.Arguments[1];
                    AddPixelEvent(new SimpleEvent(color.ToNumber4()));
                };

                // TODO: Can't use this because it only works for the back buffer.
                //rasterizerStage.FragmentFilter = (x, y) => x == pixelX.Value && y == pixelY.Value;

                outputMergerStage.ProcessedPixel += (sender, e) =>
                {
                    if (e.X != pixelX || e.Y != pixelY || e.RenderTargetArrayIndex != renderTargetArrayIndex)
                        return;

                    // TODO: Support multiple render targets.
                    DepthStencilView depthStencilView; RenderTargetView[] renderTargetViews;
                    outputMergerStage.GetTargets(out depthStencilView, out renderTargetViews);
                    if (renderTargetViews[0].ID != renderTargetViewID)
                        return;

                    var activeShader = (geometryShaderStage.Shader != null)
                        ? (ShaderBase) geometryShaderStage.Shader
                        : vertexShaderStage.Shader;

                    AddPixelEvent(new DrawEvent
                    {
                        Vertices = e.Vertices.Select(x => new DrawEventVertex
                        {
                            VertexID = x.VertexID,
                            PreVertexShaderData = MapVertexShaderData(x.InputData, activeShader.Bytecode.InputSignature),
                            PostVertexShaderData = MapVertexShaderData(x.OutputData, activeShader.Bytecode.OutputSignature)
                        }).ToArray(),
                        PrimitiveID = e.PrimitiveID,
                        X = e.X,
                        Y = e.Y,
                        PixelShader = e.PixelShader,
                        Previous = e.Previous,
                        Result = e.Result,
                        ExclusionReason = e.ExclusionReason
                    });
                };
            }
        }

        private static DrawEventVertexData[] MapVertexShaderData(
            IEnumerable<Number4> data,
            InputOutputSignatureChunk signature)
        {
            return data.Select((datum, i) =>
            {
                var parameter = signature.Parameters[i];
                
                var value = string.Empty;
                if (parameter.Mask.HasFlag(ComponentMask.X))
                    value += "x=" + datum.X + ", ";
                if (parameter.Mask.HasFlag(ComponentMask.Y))
                    value += "y=" + datum.Y + ", ";
                if (parameter.Mask.HasFlag(ComponentMask.Z))
                    value += "z=" + datum.Z + ", ";
                if (parameter.Mask.HasFlag(ComponentMask.W))
                    value += "w=" + datum.W + ", ";
                value = value.TrimEnd(' ', ',');
                return new DrawEventVertexData
                {
                    Semantic = parameter.SemanticName,
                    Value = value
                };
            }).ToArray();
        }

        private void LogOperation(OperationType type, params object[] methodArguments)
        {
            if (!_writtenBeginFrame)
            {
                _tracefile.Frames.Add(_currentFrame = new TracefileFrame
                {
                    Number = ++_frameNumber
                });
                _writtenBeginFrame = true;
            }

            _currentFrame.Events.Add(new TracefileEvent
            {
                Number = ++_operationNumber,
                OperationType = type,
                Arguments = new TracefileEventArgumentCollection(methodArguments)
            });

            if (type == OperationType.DeviceContextPresent)
            {
                _currentFrame = null;
                _writtenBeginFrame = false;
                _operationNumber = 0;
            }
        }

        private void AddPixelEvent(PixelEvent @event)
        {
            var lastEvent = _currentFrame.Events.Last();
            lastEvent.PixelEvents.Add(@event);
        }

        public IEnumerable<TracefileEvent> GetPixelHistoryEvents(int frame)
        {
            return _tracefile.Frames.Single(f => f.Number == frame).Events
                .Where(e => e.PixelEvents.Any())
                .Select(e => new TracefileEvent
                {
                    Arguments = e.Arguments,
                    Number = e.Number,
                    OperationType = e.OperationType,
                    PixelEvents = e.PixelEvents
                });
        }

        public void WriteTo(TextWriter textWriter)
        {
            _tracefile.Save(textWriter);
        }
    }
}