using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Windows.Storage;
using ParrotDiscoReflight.Code.Settings;

namespace ParrotDiscoReflight.ViewModels
{
    public class SettingsSaver : ISettingsSaver
    {
        private readonly ApplicationDataContainer settings;
        private readonly string parentTypeName;
        private readonly Type parentType;

        public SettingsSaver(object parent, ApplicationDataContainer settings)
        {
            this.settings = settings;
            parentTypeName = parent.GetType().Name;
            parentType = parent.GetType();
        }

        public T Get<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
            {
                throw new InvalidOperationException();
            }

            var settingKey = GetSettingKey(propertyName);
            if (settings.Values.TryGetValue(settingKey, out var v))
                return (T) v;
            else
                return GetDefaultValue<T>(propertyName);
        }

        public void Set<T>(T value, [CallerMemberName] string propertyName = null)
        {
            var settingKey = GetSettingKey(propertyName);
            if (settings.Values.ContainsKey(settingKey))
            {
                var currentValue = (T)settings.Values[settingKey];
                if (EqualityComparer<T>.Default.Equals(currentValue, value))
                {
                    return;
                }
            }

            settings.Values[settingKey] = value;
        }

        private string GetSettingKey(string propertyName)
        {
            return parentTypeName + "_" + propertyName;
        }

        private T GetDefaultValue<T>(string name)
        {
            var attr = parentType.GetProperty(name).GetCustomAttribute<DefaultSettingValueAttribute>();
            if (attr != null)
            {
                return (T)attr.Value;
            }

            return default(T);
        }
    }
}