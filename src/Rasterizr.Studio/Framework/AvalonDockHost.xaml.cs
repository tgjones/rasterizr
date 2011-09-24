using System;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.ComponentModel;
using AvalonDock;

namespace Rasterizr.Studio.Framework
{
    /// <summary>
    /// Interaction logic for AvalonDockHost.xaml
    /// </summary>
    public partial class AvalonDockHost : UserControl
    {
        #region Dependency Properties 

        public static readonly DependencyProperty PanesProperty =
            DependencyProperty.Register("Panes", typeof(IList), typeof(AvalonDockHost),
                new FrameworkPropertyMetadata(DocumentsOrPanes_PropertyChanged));

        public static readonly DependencyProperty DocumentsProperty =
            DependencyProperty.Register("Documents", typeof(IList), typeof(AvalonDockHost),
                new FrameworkPropertyMetadata(DocumentsOrPanes_PropertyChanged));

        public static readonly DependencyProperty ActiveDocumentProperty =
            DependencyProperty.Register("ActiveDocument", typeof(object), typeof(AvalonDockHost),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ActiveDocumentOrPane_PropertyChanged));

        public static readonly DependencyProperty ActivePaneProperty =
            DependencyProperty.Register("ActivePane", typeof(object), typeof(AvalonDockHost),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ActiveDocumentOrPane_PropertyChanged));

		public static readonly DependencyProperty DefaultLayoutProperty =
			DependencyProperty.Register("DefaultLayout", typeof(object), typeof(AvalonDockHost),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //
        // Attached properties.
        //

        public static readonly DependencyProperty IsPaneVisibleProperty =
            DependencyProperty.RegisterAttached("IsPaneVisible", typeof(bool), typeof(AvalonDockHost),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsPaneVisible_PropertyChanged));

        #endregion Dependency Properties

        #region Private Data Members

        /// <summary>
        /// A dictionary that maps from view-model object to Avalondock ManagedContent object.
        /// </summary>
        private Dictionary<object, ManagedContent> contentMap = new Dictionary<object, ManagedContent>();

        /// <summary>
        /// Set to 'true' to disable the request for user confirmation while closing a document.
        /// </summary>
        private bool disableClosingEvent = false;

        #endregion Private Data Members

        public AvalonDockHost()
        {
            InitializeComponent();

            //
            // Hook the AvalonDock event that is raised when the focused content is changed.
            //
            dockingManager.ActiveContentChanged += new EventHandler(dockingManager_ActiveContentChanged);

            UpdateActiveContent();
        }

        /// <summary>
        /// The collection of view-model objects for panes.
        /// Adding an object to this collection results in 
        /// a pane being added to AvalonDock.  There must be
        /// a DataTemplate for the view-model defined within
        /// the resources of the visual-tree and the 
        /// DataTemplate should contain an AvalonDock
        /// DockableControl.
        /// NOTE: This property is initially null, you should 
        /// either assign a collection to it or, as it is
        /// intended to be used, data-bind it to a collection
        /// in the view-model.
        /// </summary>
        public IList Panes
        {
            get
            {
                return (IList)GetValue(PanesProperty);
            }
            set
            {
                SetValue(PanesProperty, value);
            }
        }

        /// <summary>
        /// The collection of view-model objects for documents.
        /// Adding an object to this collection results in 
        /// a document being added to AvalonDock.  There must be
        /// a DataTemplate for the view-model defined within
        /// the resources of the visual-tree and the 
        /// DataTemplate should contain an AvalonDock
        /// DocumentControl.
        /// NOTE: This property is initially null, you should 
        /// either assign a collection to it or, as it is
        /// intended to be used, data-bind it to a collection
        /// in the view-model.
        /// </summary>
        public IList Documents
        {
            get
            {
                return (IList)GetValue(DocumentsProperty);
            }
            set
            {
                SetValue(DocumentsProperty, value);
            }
        }

        /// <summary>
        /// The view-model object for the currently active document.
        /// This can programatically set to change the active document,
        /// or it can be data-bound to the view-model so that the active
        /// document can be changed via the view-model.
        /// It is also used to retieve the currently active document's view-model
        /// and is automatically updated when the user interacts directly with
        /// AvalonDock selecting a tabbed-document.
        /// </summary>
        public object ActiveDocument
        {
            get
            {
                return (object)GetValue(ActiveDocumentProperty);
            }
            set
            {
                SetValue(ActiveDocumentProperty, value);
            }
        }

        /// <summary>
        /// The view-model object for the currently active pane.
        /// This can programatically set to change the active pane,
        /// or it can be data-bound to the view-model so that the active
        /// pane can be changed via the view-model.
        /// It is also used to retieve the currently active pane's view-model
        /// and is automatically updated when the user interacts directly with
        /// AvalonDock selecting a pane.
        /// </summary>
        public object ActivePane
        {
            get
            {
                return (object)GetValue(ActivePaneProperty);
            }
            set
            {
                SetValue(ActivePaneProperty, value);
            }
        }

		public object DefaultLayout
		{
			get { return GetValue(DefaultLayoutProperty); }
			set { SetValue(ActivePaneProperty, value); }
		}

        /// <summary>
        /// Allow access to the Avalondock DockingManager.
        /// This class isn't a complete wrapper, 
        /// it should be considered a helper that provides MVVM support for Avalondock.
        /// </summary>
        public DockingManager DockingManager
        {
            get
            {
                return dockingManager;
            }
        }

        /// <summary>
        /// Event raised when Avalondock has loaded.
        /// </summary>
        public event EventHandler<EventArgs> AvalonDockLoaded;

        /// <summary>
        /// Event raised when a document is being closed by clicking on the AvalonDock X button .
        /// </summary>
        public event EventHandler<DocumentClosingEventArgs> DocumentClosing;

        /// <summary>
        /// Sets the IsPaneVisible for an element.
        /// </summary>
        public static void SetIsPaneVisible(UIElement element, bool value)
        {
            element.SetValue(IsPaneVisibleProperty, value);
        }

        /// <summary>
        /// Gets the IsPaneVisible property for an element.
        /// </summary>
        public static bool GetIsPaneVisible(UIElement element)
        {
            return (bool)element.GetValue(IsPaneVisibleProperty);
        }

        #region Private Methods

        /// <summary>
        /// Event raised when the Documents or Panes properties have changed.
        /// </summary>
        private static void DocumentsOrPanes_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (AvalonDockHost)d;

            //
            // Deal with the previous value of the property.
            //
            if (e.OldValue != null)
            {
                //
                // Remove the old panels from AvalonDock.
                //
                var oldPanels = (IList)e.OldValue;
                c.RemovePanels(oldPanels);

                var observableCollection = oldPanels as INotifyCollectionChanged;
                if (observableCollection != null)
                {
                    //
                    // Unhook the CollectionChanged event, we no longer need to receive notifications
                    // of modifications to the collection.
                    //
                    observableCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.documentsOrPanes_CollectionChanged);
                }
            }

            //
            // Deal with the new value of the property.
            //
            if (e.NewValue != null)
            {
                //
                // Add the new panels to AvalonDock.
                //
                var newPanels = (IList)e.NewValue;
                c.AddPanels(newPanels);

                var observableCollection = newPanels as INotifyCollectionChanged;
                if (observableCollection != null)
                {
                    //
                    // Hook the CollectionChanged event to receive notifications
                    // of future modifications to the collection.
                    //
                    observableCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(c.documentsOrPanes_CollectionChanged);
                }
            }
        }

        /// <summary>
        /// Event raised when the 'Documents' or 'Panes' collection have had items added/removed.
        /// </summary>
        private void documentsOrPanes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //
                // The collection has been cleared, need to remove all
                // documents or panes from AvalonDock depending on which collection was
                // actually cleared.
                //
                ResetDocumentsOrPanes(sender);
            }
            else
            {
                if (e.OldItems != null)
                {
                    //
                    // Remove the old panels from AvalonDock.
                    //
                    RemovePanels(e.OldItems);
                }

                if (e.NewItems != null)
                {
                    //
                    // Add the new panels to AvalonDock.
                    //
                    AddPanels(e.NewItems);
                }
            }
        }

        /// <summary>
        /// This method resets documents or panes in response
        /// to the Reset action of the CollectionChanged events.
        /// </summary>
        private void ResetDocumentsOrPanes(object sender)
        {
            if (sender == this.Documents)
            {
                //
                // We are clearing the collection of documents.
                //
                foreach (var item in this.contentMap.ToArray())
                {
                    if (item.Value is DocumentContent)
                    {
                        //
                        // Only remove DocumentContent components.
                        //
                        RemovePanel(item.Key);
                    }
                }
            }
            else if (sender == this.Panes)
            {
                //
                // We are clearing the collection of panes.
                //
                foreach (var item in this.contentMap.ToArray())
                {
                    if (item.Value is DockableContent)
                    {
                        //
                        // Only remove DockableContent components.
                        //
                        RemovePanel(item.Key);
                    }
                }
            }
            else
            {
                throw new ApplicationException("Unexpected CollectionChanged event sender: " + sender.GetType().Name);
            }
        }

        /// <summary>
        /// Add panels to Avalondock.
        /// </summary>
        private void AddPanels(IList panels)
        {
            foreach (var panel in panels)
            {
                AddPanel(panel);
            }
        }

        /// <summary>
        /// Add a panel to Avalondock.
        /// </summary>
        private void AddPanel(object panel)
        {
            //
            // Instantiate a UI element based on the panel's view-model type.
            // The visual-tree is searched to find a DataTemplate keyed to the requested type.
            //
            var panelViewModelType = panel.GetType();
            var uiElement = DataTemplateUtils.InstanceTemplate(panelViewModelType, this, panel);
            if (uiElement == null)
            {
                throw new ApplicationException("Failed to find data-template for type: " + panel.GetType().Name);
            }

            //
            // Cast the instantiated UI element to an AvalonDock ManagedContent.
            // ManagedContent can refer to either an AvalonDock DocumentContent
            // or an AvalonDock DockableContent.
            //
            var managedContent = uiElement as ManagedContent;
            if (managedContent == null)
            {
                throw new ApplicationException("Found data-template for type: " + panel.GetType().Name + ", but the UI element generated is not a ManagedContent (base-class of DocumentContent/DockableContent), rather it is a " + uiElement.GetType().Name);
            }

            //
            // Associate the panel's view-model with the Avalondock ManagedContent so it can be retrieved later.
            //
            contentMap[panel] = managedContent;

            //
            // Hook the event to track when the document has been closed.
            //
            managedContent.Closed += new EventHandler(managedContent_Closed);

		    var documentContent = managedContent as DocumentContent;
		    if (documentContent != null)
		    {
			    //
			    // For documents only, hook Closing so that the application can be informed
			    // when a document is in the process of being closed by the use clicking the
			    // AvalonDock close button.
			    //
			    documentContent.Closing += new EventHandler<CancelEventArgs>(documentContent_Closing);
		    }
		    else
		    {
			    var dockableContent = managedContent as DockableContent;
			    if (dockableContent != null)
			    {
				    //
				    // For panes only, hook StateChanged so we know when a DockableContent is shown/hidden.
				    //
				    dockableContent.StateChanged += new RoutedEventHandler(dockableContent_StateChanged);
			    }
			    else
			    {
                    throw new ApplicationException("Panel " + managedContent.GetType().Name + " is expected to be either DocumentContent or DockableContent."); 
			    }
		    }

            managedContent.Show(dockingManager);
            managedContent.Activate();
        }

        /// <summary>
        /// Remove panels from Avalondock.
        /// </summary>
        private void RemovePanels(IList panels)
        {
            foreach (var panel in panels)
            {
                RemovePanel(panel);
            }
        }

        /// <summary>
        /// Remove a panel from Avalondock.
        /// </summary>
        private void RemovePanel(object panel)
        {
            //
            // Look up the document in the content map.
            //
            ManagedContent managedContent = null;
            if (contentMap.TryGetValue(panel, out managedContent))
            {
                disableClosingEvent = true;

                try
                {
                    //
                    // The content was still in the map, and therefore still open, so close it.
                    //
                    managedContent.Close();
                }
                finally
                {
                    disableClosingEvent = false;
                }
            }
        }

        /// <summary>
        /// Event raised when an AvalonDock DocumentContent is being closed.
        /// </summary>
        private void documentContent_Closing(object sender, CancelEventArgs e)
        {
            var documentContent = (DocumentContent)sender;
            var document = documentContent.DataContext;

            if (!disableClosingEvent)
            {
                if (this.DocumentClosing != null)
                {
                    //
                    // Notify the application that the document is being closed.
                    //
                    var eventArgs = new DocumentClosingEventArgs(document);
                    this.DocumentClosing(this, eventArgs);

                    if (eventArgs.Cancel)
                    {
                        //
                        // Closing of the document is to be cancelled.
                        //
                        e.Cancel = true;
                        return;
                    }
                }
            }

            documentContent.Closing -= new EventHandler<CancelEventArgs>(documentContent_Closing);
        }

        /// <summary>
        /// Event raised when an Avalondock ManagedContent has been closed.
        /// </summary>
        private void managedContent_Closed(object sender, EventArgs e)
        {
            var managedContent = (ManagedContent)sender;
            var content = managedContent.DataContext;

            //
            // Remove the content from the content map right now.
            // There is no need to keep it around any longer.
            //
            contentMap.Remove(content);

            managedContent.Closed -= new EventHandler(managedContent_Closed);

            var documentContent = managedContent as DocumentContent;
            if (documentContent != null)
            {
                this.Documents.Remove(content);

                if (this.ActiveDocument == content)
                {
                    //
                    // Active document has closed, clear it.
                    //
                    this.ActiveDocument = null;
                }
            }
            else
            {
                var dockableContent = managedContent as DockableContent;
                if (dockableContent != null)
                {
                    //
                    // For panes only, unhook StateChanged event.
                    //
                    dockableContent.StateChanged -= new RoutedEventHandler(dockableContent_StateChanged);

                    this.Panes.Remove(content);

                    if (this.ActivePane == content)
                    {
                        //
                        // Active pane has closed, clear it.
                        //
                        this.ActivePane = null;
                    }
                }
            }
        }

        /// <summary>
        /// Event raised when the 'dockable state' of a DockableContent has changed.
        /// </summary>
        private void dockableContent_StateChanged(object sender, RoutedEventArgs e)
        {
            var dockableContent = (DockableContent)sender;
            SetIsPaneVisible(dockableContent, dockableContent.State != DockableContentState.Hidden);
        }

        /// <summary>
        /// Update the active pane and document from the currently active AvalonDock component.
        /// </summary>
        private void UpdateActiveContent()
        {
            var activePane = dockingManager.ActiveContent as DockableContent;
            if (activePane != null)
            {
                //
                // Set the active document so that we can bind to it.
                //
                this.ActivePane = activePane.DataContext;
            }
            else
            {
                var activeDocument = dockingManager.ActiveContent as DocumentContent;
                if (activeDocument != null)
                {
                    //
                    // Set the active document so that we can bind to it.
                    //
                    this.ActiveDocument = activeDocument.DataContext;
                }
            }
        }

        /// <summary>
        /// Event raised when the IsPaneVisible property changes.
        /// </summary>
        private static void IsPaneVisible_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var avalonDockContent = o as ManagedContent;
            if (avalonDockContent != null)
            {
                bool isVisible = (bool)e.NewValue;
                if (isVisible)
                {
                    avalonDockContent.Show();
                }
                else
                {
                    avalonDockContent.Hide();
                }
            }
        }

        /// <summary>
        /// Event raised when the active content has changed.
        /// </summary>
        private void dockingManager_ActiveContentChanged(object sender, EventArgs e)
        {
            UpdateActiveContent();
        }

        /// <summary>
        /// Event raised when the ActiveDocument or ActivePane property has changed.
        /// </summary>
        private static void ActiveDocumentOrPane_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (AvalonDockHost)d;
            ManagedContent managedContent = null;
            if (e.NewValue != null &&
                c.contentMap.TryGetValue(e.NewValue, out managedContent))
            {
                managedContent.Activate();
            }
        }

        /// <summary>
        /// Event raised when Avalondock has loaded.
        /// </summary>
        private void AvalonDock_Loaded(object sender, RoutedEventArgs e)
        {
            if (AvalonDockLoaded != null)
            {
                AvalonDockLoaded(this, EventArgs.Empty);
            }

			// Load default layout - could replace this with loading from a saved file.
        	dockingManager.Content = DefaultLayout;
        }

        #endregion Private Methods
    }
}
