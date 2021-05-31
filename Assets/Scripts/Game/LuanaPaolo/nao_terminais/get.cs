using UnityEngine;

public class get
{
    float r;

    public void option(Manager m, int lim, int[] pesos)
    {
        r = ((pesos[0] + pesos[1] * 2 + pesos[2] * 3 + pesos[3] * 4) / 16) * Random.Range(0f, 3f);
        if (lim == 3) r = 2.8f;

        lim++;

        if (r > 2.85)
        {
            get_ter g = new get_ter();
            g.choose(m);
            talk t = new talk();
            t.option(m, lim, pesos);
            this.option(m, lim, pesos);
        }
        if (r > 2.7 && r <= 2.85)
        {
            get_ter g = new get_ter();
            g.choose(m);
        }
        if (r > 2.55 && r <= 2.7)
        {
            drop_ter d = new drop_ter();
            d.choose(m);
            talk t = new talk();
            t.option(m, lim, pesos);
            this.option(m, lim, pesos);
        }
        if (r > 2.4 && r <= 2.55)
        {
            drop_ter d = new drop_ter();
            d.choose(m);
        }
        if (r > 2.25 && r <= 2.4)
        {
            chest_ter c = new chest_ter();
            c.choose(m);
            talk t = new talk();
            t.option(m, lim, pesos);
            this.option(m, lim, pesos);
        }
        if (r <= 2.25)
        {
            chest_ter c = new chest_ter();
            c.choose(m);
        }
    }
}