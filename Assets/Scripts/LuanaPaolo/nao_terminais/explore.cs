using UnityEngine;

public class explore
{
    float r;

    public void option(Manager m, int lim, int[] pesos)
    {
        r = ((pesos[0] + pesos[1] * 2 + pesos[2] * 3 + pesos[3] * 4) / 16) * Random.Range(0f, 3f);
        if (lim == 3) r = 2.5f;

        lim++;

        if (r > 2.85)
        {
            explore_ter e = new explore_ter();
            e.choose(m);
            talk t = new talk();
            t.option(m, lim, pesos);
            this.option(m, lim, pesos);
        }
        if (r > 2.6 && r <= 2.85)
        {
            explore_ter e = new explore_ter();
            e.choose(m);
        }
        if (r > 2.35 && r <= 2.6)
        {
            secret_ter s = new secret_ter();
            s.choose(m);
            talk t = new talk();
            t.option(m, lim, pesos);
            this.option(m, lim, pesos);
        }
        if (r <= 2.35)
        {
            secret_ter s = new secret_ter();
            s.choose(m);
        }
    }
}