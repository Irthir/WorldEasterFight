//BUT : Conceptualiser un objet d'action pour le réseau
public struct AttaqueV3
{
    public AttaqueV3(int case1, int case2, int case3)
    {
        Case1 = case1;
        Case2 = case2;
        Case3 = case3;
    }

    public int Case1 { get; }
    public int Case2 { get; }
    public int Case3 { get; }

    public override string ToString()
    {
        return Case1.ToString() + "," + Case2.ToString() + "," + Case3.ToString();
    }
}
