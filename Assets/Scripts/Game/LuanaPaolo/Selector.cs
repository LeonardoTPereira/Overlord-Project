//classe que seleciona a linha de missões de acordo com os pesos do perfil do jogador
public class Selector
{

    private float r;
    public int[] pesos = new int[4]; //vetor de pesos

    /*
    [7][5][1][3]
    [3][7][1][5]
    [1][5][7][3]
    [1][5][3][7]
    */

    public void select(Manager m)
    {
        pesos[0] = 3; //peso talk
        pesos[1] = 7; //peso get
        pesos[2] = 1; //peso kill
        pesos[3] = 5; //peso explore

        //r = Random.Range(0, 4);
        r = 2.7f;//((pesos[0] + pesos[1]*2 + pesos[2]*3 + pesos[3]*4)/16);// * Random.Range(0f, 3f); <<-- equação ainda inutilizada devido a testes específicos

        if (r <= 2.35)
        {
            talk t = new talk();
            t.option(m, 0, pesos);
        }
        if (r > 2.35 && r <= 2.6)
        {
            get g = new get();
            g.option(m, 0, pesos);
        }
        if (r > 2.6 && r <= 2.85)
        {
            kill k = new kill();
            k.option(m, 0, pesos);
        }
        if (r > 2.85)
        {
            explore e = new explore();
            e.option(m, 0, pesos);
        }
    }
}