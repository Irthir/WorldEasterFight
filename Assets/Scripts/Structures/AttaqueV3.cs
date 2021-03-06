//BUT : Conceptualiser un objet d'action pour le r?seau
public struct AttaqueV3
{
    public AttaqueV3(int type, int case1, int case2, int case3)
    {
        Type = type % 5;
        Case1 = case1 % 9;
        Case2 = case2 % 9;
        Case3 = case3 % 9;
    }

    public int Type { get; }
    public int Case1 { get; }
    public int Case2 { get; }
    public int Case3 { get; }

    public override string ToString()
    {
        return Type.ToString() + "," + Case1.ToString() + "," + Case2.ToString() + "," + Case3.ToString();
    }

    public static AttaqueV3 FromString(string sAttaqueV3)
    {
        int n0 = int.Parse(sAttaqueV3.Substring(0, 1));
        int n1 = int.Parse(sAttaqueV3.Substring(2, 1));
        int n2 = int.Parse(sAttaqueV3.Substring(4, 1));
        int n3 = int.Parse(sAttaqueV3.Substring(6, 1));

        return new AttaqueV3(n0, n1, n2, n3);
    }
}
