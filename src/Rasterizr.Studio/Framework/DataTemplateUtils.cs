using System;
using System.Windows;
using System.Windows.Data;
using AvalonDock;

namespace Rasterizr.Studio.Framework
{
    /// <summary>
    /// Helper class for lookup of data-templates.
    /// </summary>
    internal static class DataTemplateUtils
    {
        /// <summary>
        /// Find a DataTemplate for the specified type in the visual-tree.
        /// </summary>
        public static DataTemplate FindDataTemplate(Type type, FrameworkElement element)
        {
            var dataTemplate = element.TryFindResource(new DataTemplateKey(type)) as DataTemplate;
            if (dataTemplate != null)
            {
                return dataTemplate;
            }

            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                dataTemplate = FindDataTemplate(type.BaseType, element);
                if (dataTemplate != null)
                {
                    return dataTemplate;
                }
            }

            foreach (var interfaceType in type.GetInterfaces())
            {
                dataTemplate = FindDataTemplate(interfaceType, element);
                if (dataTemplate != null)
                {
                    return dataTemplate;
                }
            }

            return null;
        }

		private static DataTemplate CreateDataTemplate(object dataContext)
		{
			if (dataContext is DocumentViewModelBase)
			{
				var factory = new FrameworkElementFactory(typeof (DocumentContent));
				factory.SetBinding(ManagedContent.TitleProperty, new Binding("Title"));

				var viewType = Type.GetType(dataContext.GetType().FullName.Replace("ViewModel", "View"));
				var viewFactory = new FrameworkElementFactory(viewType);
				factory.AppendChild(viewFactory);

				var dataTemplate = new DataTemplate
				{
					DataType = dataContext.GetType(),
					VisualTree = factory
				};
				dataTemplate.Seal();
				return dataTemplate;
			}
			throw new NotImplementedException();
		}

    	/// <summary>
        /// Find a data-template for the specified type and instance a visual from it.
        /// </summary>
        public static FrameworkElement InstanceTemplate(Type type, FrameworkElement element, object dataContext)
        {
			var dataTemplate = CreateDataTemplate(dataContext);
    		if (dataTemplate == null)
    			return null;

    		return InstanceTemplate(dataTemplate, dataContext);
        }

        /// <summary>
        /// Instance a visual element from a data template.
        /// </summary>
        public static FrameworkElement InstanceTemplate(DataTemplate dataTemplate, object dataContext)
        {
            var element = (FrameworkElement)dataTemplate.LoadContent();
            element.DataContext = dataContext;
            return element;
        }
    }
}
