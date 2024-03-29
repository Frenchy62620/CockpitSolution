﻿using CockpitBuilder.Common.PropertyEditors;
using CockpitBuilder.Events;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CockpitBuilder.Common.CustomControls;
using IEventAggregator = CockpitBuilder.Core.Common.Events.IEventAggregator;

namespace CockpitBuilder.Plugins.General
{
    public class PushButton_ViewModel : PluginModel  /*Core.Common.Events.IHandle<EditEvent>,*/
                                                     //Core.Common.Events.IHandle<TransformEvent>,
                                                     //Core.Common.Events.IHandle<SelectedEvent>,
                                                     //Core.Common.Events.IHandle<NewLayoutEvent>,
                                                     //Core.Common.Events.IHandle<NewAppearanceEvent>,
                                                     //IPlugin
    {
        private readonly IEventAggregator eventAggregator;

        public PushButtonAppearanceEditorViewModel Appearance { get; }
        public LayoutPropertyEditorViewModel Layout { get; }
        public PushButtonBehaviorEditorViewModel Behavior { get; }

        public PushButton_ViewModel(IEventAggregator eventAggregator, params object[] settings)
        {
            Layout = new LayoutPropertyEditorViewModel(eventAggregator, settings);
            Appearance = new PushButtonAppearanceEditorViewModel(eventAggregator, settings);
            Behavior = new PushButtonBehaviorEditorViewModel(eventAggregator, settings);


            NameUC = (string)settings[1];


            //Frame = false;

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }

        #region PluginModel
        public override double Left
        {
            get => Layout.UCLeft;
            set => Layout.UCLeft = value;
        }
        public override double Top
        {
            get => Layout.UCTop;
            set => Layout.UCTop = value;
        }
        public override double Width
        {
            get => Layout.Width;
            set => Layout.Width = value;
        }
        public override double Height
        {
            get => Layout.Height;
            set => Layout.Height = value;
        }
        //public override double Angle
        //{
        //    get => Layout.AngleRotation;
        //    set => Layout.AngleRotation = value;
        //}
        public override PropertyEditorModel[] GetProperties()
        {
            return new PropertyEditorModel[] { Layout, Appearance, Behavior };
        }
        #endregion


        ~PushButton_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine("sortie push");
        }
        #region Selection Image

        #endregion


        #region Mouse Events
        public void MouseLeftButtonDownOnUC(IInputElement elem, Point pos, MouseEventArgs e)
        {
            Mouse.Capture((UIElement)elem);
            Appearance.IndexImage = 1 - Appearance.IndexImage;
        }
        public void MouseLeftButtonUp()
        {
            Mouse.Capture(null);
            if ((int)Behavior.SelectedPushButtonType == 0)
                Appearance.IndexImage = 0;
        }
        public void MouseEnterInUC(MouseEventArgs e)
        {
            /*{Appearance.Center})\n({ScaleX:0.##}*/
            //ToolTip = $"({Layout.UCLeft}, , {(ScaleX * Appearance.GlyphThickness):0.##}, {(ScaleX * Layout.Height):0.##}), Tag = {tag}";
        }
        #endregion

        #region Mode Edition
        //private bool _frame;
        //public bool Frame
        //{
        //    get { return _frame; }
        //    set
        //    {
        //        _frame = value;
        //        NotifyOfPropertyChange(() => Frame);
        //    }
        //}
        //private bool _isSelected;
        //public bool IsSelected
        //{
        //    get { return _isSelected; }
        //    set
        //    {
        //        _isSelected = value;
        //        NotifyOfPropertyChange(() => IsSelected);
        //    }
        //}
        #endregion

        #region Scale, rotation, XY translation usercontrol
        private double _scalex;
        public double ScaleX
        {
            get { return _scalex; }
            set
            {
                _scalex = value;
                NotifyOfPropertyChange(() => ScaleX);
            }
        }
        #endregion

        #region ToolTip
        private string _tooltip;
        public string ToolTip
        {
            get { return _tooltip; }
            set
            {
                _tooltip = value;
                NotifyOfPropertyChange(() => ToolTip);
            }
        }
        #endregion



        #region HandleEvents


        public void Handle(TransformEvent translate)
        {
            //if (translate.fromProperty)
            //{
            //    UCLeft = translate.DeltaLeft;
            //    CenterCircle = translate.DeltaTop;
            //    return;
            //}
            //if (!IsSelected) return;
            //if (translate.Size == 0)
            //{
            //    UCLeft += translate.DeltaLeft;
            //    CenterCircle += translate.DeltaTop;
            //    return;
            //}
            //ScaleX = ScaleX * (translate.Size + GlyphThickness) / GlyphThickness;
        }




        public void Handle(NewAppearanceEvent message)
        {
            //pushButtonAppearance = (PushButtonAppearance) message.Appearance;

        }
        #endregion
    }
}
