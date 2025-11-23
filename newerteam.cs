using ScriptPortal.Vegas;

public class EntryPoint
{
    public void FromVegas(Vegas vegas)
    {
        foreach (Track track in vegas.Project.Tracks)
        {
            foreach (TrackEvent trkEvent in track.Events)
            {
                if (!(trkEvent is VideoEvent ev)) continue;

                // ----- POSTERIZE -----
                var posterize = vegas.VideoFX["NewerTeam Posterize"];
                var pInst = ev.Effects.AddEffect(posterize);
                pInst.ParameterByName("Levels").Value = 5;

                // ----- WAVE -----
                var wave = vegas.VideoFX["NewerTeam Wave"];
                var wInst = ev.Effects.AddEffect(wave);
                var freq = wInst.ParameterByName("Frequency") as OFXDoubleParameter;
                var freqAuto = freq.Automation;
                freqAuto.Points.Add(new OFXAutomationPoint(Timecode.FromSeconds(0), 0));
                freqAuto.Points.Add(new OFXAutomationPoint(Timecode.FromSeconds(1), 1));

                // ----- VIBRATO -----
                var vib = vegas.VideoFX["NewerTeam Vibrato"];
                var vInst = ev.Effects.AddEffect(vib);
                vInst.ParameterByName("Amount").Value = 0.5;

                // ----- SPEED (Time Stretch) -----
                ev.PlaybackRate = 1.5;    // 150% speed

                // ----- BEZIER CURVE (generic example) -----
                var amount = vInst.ParameterByName("Amount") as OFXDoubleParameter;
                var auto = amount.Automation;

                var a0 = auto.Points.Add(new OFXAutomationPoint(Timecode.FromSeconds(0), 0.0));
                var a1 = auto.Points.Add(new OFXAutomationPoint(Timecode.FromSeconds(1), 1.0));

                a0.Type = OFXAutomationPointType.Bezier;
                a1.Type = OFXAutomationPointType.Bezier;

                a0.RightBezier = 0.5;
                a1.LeftBezier = 0.3;
            }
        }
    }
}
