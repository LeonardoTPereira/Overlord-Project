using UnityEngine;

public class kill
{
    float r, b;
    public void option(Manager m, int lim, int[] pesos)
    {
        r = ((pesos[0] + pesos[1] * 2 + pesos[2] * 3 + pesos[3] * 4) / 16) * Random.Range(0f, 3f);
        if (lim == 3) r = 2.5f;

        b = Random.Range(0, 100);

        lim++;

        kill_ter k = new kill_ter();

        if (r <= 2.5)
        {
            k.choose(m);
            talk t = new talk();
            t.option(m, lim, pesos);
            this.option(m, lim, pesos);
        }
        if (r > 2.5) k.choose(m);
    }
}