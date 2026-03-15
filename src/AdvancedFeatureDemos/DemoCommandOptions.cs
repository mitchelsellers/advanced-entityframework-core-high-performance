using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdvancedFeatureDemos;

public enum DemoCommandOptions
{
    [Display(Name = "List Commands")]
    ListCommands = 1,
    [Display(Name = "Randomly Update Speaker Session Counts")]
    RandomlyUpdateSpeakerSessionCounts = 2,
    [Display(Name = "List Current Speaker Data")]
    ListCurrentSpeakerData = 3,

    [Display(Name = "Exit")]
    Exit = 99
}