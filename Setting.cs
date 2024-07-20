using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using System.Collections.Generic;

namespace InstantBoarding
{
    [FileLocation(nameof(InstantBoarding))]
    [SettingsUIGroupOrder(carMaxDwellDelay, trainMaxDwellDelay)]
    [SettingsUIShowGroupName(carMaxDwellDelay, trainMaxDwellDelay)]
    public class Setting : ModSetting
    {
        public const string sectionName = "Dwell Delay Configuration";
        public const string groupName = "Maximum Dwell Delay (s) [Beyond Scheduled Departure Time]";
        public const string carMaxDwellDelay = "Car Transport Types (Bus, Taxi, etc)";
        public const string trainMaxDwellDelay = "Train Transport Types (Train, Subway, Tram)";

        public Setting(IMod mod) : base(mod)
        {
            SetDefaults();
        }

        [SettingsUISlider(min = 0, max = 300, step = 5, scalarMultiplier = 1, unit = Unit.kDataMegabytes)]
        [SettingsUISection(sectionName, groupName)]
        public uint CarMaxDwellDelaySlider { get; set; }
        
        [SettingsUISlider(min = 0, max = 300, step = 5, scalarMultiplier = 1, unit = Unit.kDataMegabytes)]
        [SettingsUISection(sectionName, groupName)]
        public uint TrainMaxDwellDelaySlider { get; set; }


        public override void SetDefaults()
        {
            CarMaxDwellDelaySlider = 30;
            TrainMaxDwellDelaySlider = 30;
        }

        public enum SomeEnum
        {
            Value1,
            Value2,
            Value3,
        }
    }

    // public class LocaleEN : IDictionarySource
    // {
    //     private readonly Setting m_Setting;
    //     public LocaleEN(Setting setting)
    //     {
    //         m_Setting = setting;
    //     }
    //     public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
    //     {
    //         return new Dictionary<string, string>
    //         {
    //             { m_Setting.GetSettingsLocaleID(), "InstantBoarding" },
    //             { m_Setting.GetOptionTabLocaleID(Setting.sectionName), "Main" },
    //
    //             { m_Setting.GetOptionGroupLocaleID(Setting.groupName), "Buttons" },
    //             { m_Setting.GetOptionGroupLocaleID(Setting.kToggleGroup), "Toggle" },
    //             { m_Setting.GetOptionGroupLocaleID(Setting.carMaxDwellDelay), "Sliders" },
    //             { m_Setting.GetOptionGroupLocaleID(Setting.kDropdownGroup), "Dropdowns" },
    //
    //             { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Button)), "Button" },
    //             { m_Setting.GetOptionDescLocaleID(nameof(Setting.Button)), $"Simple single button. It should be bool property with only setter or use [{nameof(SettingsUIButtonAttribute)}] to make button from bool property with setter and getter" },
    //
    //             { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ButtonWithConfirmation)), "Button with confirmation" },
    //             { m_Setting.GetOptionDescLocaleID(nameof(Setting.ButtonWithConfirmation)), $"Button can show confirmation message. Use [{nameof(SettingsUIConfirmationAttribute)}]" },
    //             { m_Setting.GetOptionWarningLocaleID(nameof(Setting.ButtonWithConfirmation)), "is it confirmation text which you want to show here?" },
    //
    //             { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Toggle)), "Toggle" },
    //             { m_Setting.GetOptionDescLocaleID(nameof(Setting.Toggle)), $"Use bool property with setter and getter to get toggable option" },
    //
    //             { m_Setting.GetOptionLabelLocaleID(nameof(Setting.IntSlider)), "Int slider" },
    //             { m_Setting.GetOptionDescLocaleID(nameof(Setting.IntSlider)), $"Use int property with getter and setter and [{nameof(SettingsUISliderAttribute)}] to get int slider" },
    //
    //             { m_Setting.GetOptionLabelLocaleID(nameof(Setting.IntDropdown)), "Int dropdown" },
    //             { m_Setting.GetOptionDescLocaleID(nameof(Setting.IntDropdown)), $"Use int property with getter and setter and [{nameof(SettingsUIDropdownAttribute)}(typeof(SomeType), nameof(SomeMethod))] to get int dropdown: Method must be static or instance of your setting class with 0 parameters and returns {typeof(DropdownItem<int>).Name}" },
    //
    //             { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnumDropdown)), "Simple enum dropdown" },
    //             { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnumDropdown)), $"Use any enum property with getter and setter to get enum dropdown" },
    //
    //             { m_Setting.GetEnumValueLocaleID(Setting.SomeEnum.Value1), "Value 1" },
    //             { m_Setting.GetEnumValueLocaleID(Setting.SomeEnum.Value2), "Value 2" },
    //             { m_Setting.GetEnumValueLocaleID(Setting.SomeEnum.Value3), "Value 3" },
    //
    //         };
    //     }
    //
    //     public void Unload()
    //     {
    //
    //     }
    // }
}
