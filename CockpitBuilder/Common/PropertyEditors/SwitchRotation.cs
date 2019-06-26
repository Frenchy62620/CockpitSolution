using System.ComponentModel;

namespace CockpitBuilder.Common.PropertyEditors
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SwitchRotation
    {
        None,
        [Description("90° Clockwise")]
        CW = 90,
        [Description("180° Inversed")]
        INV = 180,
        [Description("90° Counter Clockwise")]
        CCW = 270,
    }
}
