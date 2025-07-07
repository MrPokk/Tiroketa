using BitterCMS.Utility.Interfaces;
using System;

namespace Game._Script._Cms_Content.Components
{
    [Serializable]
    public class HealthComponent : IEntityComponent
    {
        private int _maxHealth;
        private int _healthCurrent;
        
        public int MaxHealth {
            get => _maxHealth;
            set => Init(value);
        }
        
        public int HealthCurrent => _healthCurrent;

        public void Decrease(int value)
        {
            var deltaHealth = _healthCurrent - value;
            _healthCurrent = deltaHealth >= 0 ? deltaHealth : 0;
        }

        public void Increase(int value)
        {
            var deltaHealth = value + _healthCurrent;
            _healthCurrent = deltaHealth >= _maxHealth ? _maxHealth : deltaHealth;
        }

        public void Init(int maxHealth)
        {
            if (maxHealth >= 0)
                _maxHealth = maxHealth;
            else
                throw new ArgumentException("ERROR: THE VALUES MUST BE GREATER THAN 0");
        
            _healthCurrent = _maxHealth;
        }
    }
}
