Rasterizr
=========

Rasterizr is a software rasterizer written in C#. It adheres closely to the Direct3D 10/11 API, 
but is completely implemented in software. If you're familiar with SharpDX and / or Direct3D 10/11, you'll be immediately at home with
Rasterizr's API.

Rasterizr is primarily intended as an educational tool. Its inner workings are roughly comparable 
to a modern GPU, so by studying how Rasterizr works, you will get a good idea of what your 
GPU is doing when you use a hardware API like Direct3D or OpenGL.

Although its primary goal is a didactic one, Rasterizr is quite powerful in its own right.
See below for a list of implemented features.

Why?
----

Why not?


Features
--------

* Borrows much of its API from Direct3D 10, including the pipeline architecture:
  * Input assembler stage
  * Vertex shader stage
  * Geometry shader stage
  * Rasterizer stage
  * Pixel shader stage
  * Output merger stage
* Uses "real" HLSL shaders in the vertex shader, geometry shader, and pixel shader stages.
  Rasterizr uses [SlimShader](https://github.com/tgjones/slimshader) to parse HLSL bytecode, 
  and SlimShader.VirtualMachine to execute HLSL shaders on the CPU.
* Textures can be bound to the pipeline using render target views, depth buffer views, 
  and shader resource views. The environment mapping sample uses this feature to bind a texture
  array as a cubemap render target, then uses the geometry shader to write to all 6 faces in
  the same draw call, and finally binds the cubemap as a shader resource to be read from by
  the pixel shader.
* Record all calls in a frame to a file for playback later. You can also load these "tracefiles" 
  within Rasterizr Studio and use visual debugging tools, including a pixel history tool.
* Vertex data is supplied using user-defined structs, and can include an arbitrary number of per-vertex attributes.
* System-value semantics, such as SV_PrimitiveID, are supported.
* Pipeline configuration is organised into state objects.
* Barycentric triangle rasterization, with perspective-correct attribute interpolation.
* Input assembler supports indexing and instancing.
* Pixel shader uses partial derivatives, calculated from a 2x2 fragment quad, to properly implement texture mipmapping.
* Bilinear and anisotropic texture filtering.
* Output merger supports full range of blend operations.


Example
-------

The following code is from the [Basic Triangle sample](https://github.com/tgjones/rasterizr/blob/master/src/Rasterizr.Studio/Modules/SampleBrowser/Samples/BasicTriangle/BasicTriangleSample.cs).
If you know SharpDX, then this code should look very familiar to you! But everything happens on the CPU,
in managed code - in fact, `Rasterizr.dll` is a Portable Class Library (PCL).

```csharp
const int width = 600;
const int height = 400;

// Create device and swap chain.
var swapChainPresenter = new WpfSwapChainPresenter();
var swapChain = device.CreateSwapChain(width, height, swapChainPresenter);
var deviceContext = device.ImmediateContext;

// Create RenderTargetView from the backbuffer.
var backBuffer = Texture2D.FromSwapChain(swapChain, 0);
var renderTargetView = device.CreateRenderTargetView(backBuffer);

// Compile Vertex and Pixel shaders
var vertexShaderByteCode = ShaderCompiler.CompileFromFile("MiniTri.fx", "VS", "vs_4_0");
var vertexShader = device.CreateVertexShader(vertexShaderByteCode);

var pixelShaderByteCode = ShaderCompiler.CompileFromFile("MiniTri.fx", "PS", "ps_4_0");
var pixelShader = device.CreatePixelShader(pixelShaderByteCode);

// Layout from VertexShader input signature
var layout = device.CreateInputLayout(
  new[]
  {
    new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0),
    new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 0)
  }, vertexShaderByteCode);

// Instantiate Vertex buffer from vertex data
var vertices = device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), new[]
{
  new Vector4(0.0f, 0.5f, 0.5f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
  new Vector4(0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
  new Vector4(-0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
});

var model = new Model();
model.AddMesh(new ModelMesh
{
  IndexBuffer = device.CreateBuffer(new BufferDescription
  {
    BindFlags = BindFlags.IndexBuffer
  }, new uint[] { 0, 1, 2 }),
  IndexCount = 3,
  InputLayout = layout,
  PrimitiveCount = 1,
  PrimitiveTopology = PrimitiveTopology.TriangleList,
  VertexBuffer = vertices,
  VertexCount = 3,
  VertexSize = 32
});

// Prepare all the stages
deviceContext.VertexShader.Shader = vertexShader;
deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, width, height, 0.0f, 1.0f));
deviceContext.PixelShader.Shader = pixelShader;
deviceContext.OutputMerger.SetTargets(null, renderTargetView);

// Render!
deviceContext.ClearRenderTargetView(renderTargetView, Color4.Black);
model.Draw(deviceContext);
deviceContext.Present(swapChain);
```

That code, along with a little more plumbing code, results in this:

![Screenshot](https://github.com/tgjones/rasterizr/raw/master/doc/rasterizr-studio-triangle.png)

Rasterizr Studio
----------------

Rasterizr Studio serves two purposes:

1. It is a sample browser, showcasing a growing list of Rasterizr samples.
2. It includes a visual debugger, integrated with the sample browser. When viewing
   any of the samples, you can pause them and open the current frame in the debugging tools.
   From there, you can click on a pixel to see the complete pixel history, as well as viewing
   all the events that make up that frame, and the resources that were used to render that frame.
   The debugging tools include:
   * Event list
   * Object table
   * Pixel history   

![Screenshot](https://github.com/tgjones/rasterizr/raw/master/doc/rasterizr-studio.png)

Acknowledgements
----------------

While writing Rasterizr, I studied the code of several other software rasterizers:

* [SoftRender](http://softrender.codeplex.com/) - the author, Thiago Pastor, kindly shared the list of resources
  that he referred to whilst writing SoftRender.
* [Trenki's software renderer](http://www.trenki.net/content/view/18/38/) - Markus Trenkwalder kindly responded
  to my queries - although that was back in 2010, so you can see how long I've been working on Rasterizr!
* [DShade](http://h3.gd/code/) - Tomasz Stachowiak wrote this software rasterizer in the 
  D programming language.
  
To learn how to implement the rasterizer stage, I read a lot of papers / books / blog posts! Here's a 
few that I remember:

* [Jim Blinn's Corner: A Trip Down the Graphics Pipeline](http://www.amazon.com/Jim-Blinns-Corner-Graphics-Pipeline/dp/1558603875)
* [A trip through the Graphics Pipeline 2011](http://fgiesen.wordpress.com/2011/07/09/a-trip-through-the-graphics-pipeline-2011-index/)
* [Optimizing Software Occlusion Culling](http://fgiesen.wordpress.com/2013/02/17/optimizing-sw-occlusion-culling-index/)
* [Software Rasterizer Part 1](http://www.altdevblogaday.com/2012/04/14/software-rasterizer-part-1/) 
  and [Part 2](http://www.altdevblogaday.com/2012/04/29/software-rasterizer-part-2/)
  
Rasterizr Studio includes some samples ported from 
[SharpDX](https://github.com/sharpdx/SharpDX/tree/master/Samples). The environment mapping
sample was ported from RobyDX's [SharpDX demos](https://github.com/RobyDX/SharpDX_Demo).
  
Disclaimer
----------

Rasterizr is not feature-complete. It is a hobby project, and I've only implemented
as much as I needed to in order to get the samples working. If you find a bug or missing feature,
and you feel like digging into the code, then go for it - I'll probably accept any pull requests.
But I don't intend to spend much more time on it myself. 3 years (on and mostly off) is long enough!

License
-------

Rasterizr is released under the [MIT License](http://www.opensource.org/licenses/MIT).