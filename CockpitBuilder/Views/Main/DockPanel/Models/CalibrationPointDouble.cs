using Caliburn.Micro;

namespace CockpitBuilder.Views.Main.DockPanel.Models
{
    public class CalibrationPointDouble : PropertyChangedBase
    {
        public CalibrationPointDouble(double input, double outputValue)
        {
            Value = input;
            Multiplier = outputValue;
        }

        private double _input;
        public double Value
        {
            get => _input;
            set
            {
                _input = value;
                NotifyOfPropertyChange(() => Value);
            }
        }

        private double _output;
        public double Multiplier
        {
            get => _output;
            set
            {
                _output = value;
                NotifyOfPropertyChange(() => Multiplier);
            }
        }
    }
}
