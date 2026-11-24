using UnityEngine;

// One base hue (168 deg, teal) and its triadic partners (288 deg, 48 deg).
// Dark / mid / light values keep apart in grayscale (gray 37 / 158 / 242).
public static class Palette
{
    public static readonly Color Panel      = new Color32(0x0F, 0x24, 0x20, 255); // dark  #0F2420
    public static readonly Color PanelLight = new Color32(0x1D, 0x3F, 0x38, 255); // track #1D3F38
    public static readonly Color Mid        = new Color32(0x34, 0xB2, 0x99, 255); // mid   #34B299
    public static readonly Color Light      = new Color32(0xEB, 0xF4, 0xF2, 255); // light #EBF4F2
    public static readonly Color Score      = new Color32(0xFA, 0xD5, 0x42, 255); // triadic 48 deg  #FAD542
    public static readonly Color Pickup     = new Color32(0xE0, 0x9D, 0xF1, 255); // triadic 288 deg #E09DF1
    public static readonly Color LagBar     = new Color32(0xB7, 0xE3, 0xDA, 255); // paler than the fill
    public static readonly Color Locked     = new Color32(0x22, 0x46, 0x3F, 255);

    public static Color Hover => Color.Lerp(Mid, Light, 0.35f); // dark label still >= 4.5 : 1
}
