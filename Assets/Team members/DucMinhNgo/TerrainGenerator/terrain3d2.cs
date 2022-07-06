using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrain3d2 : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Terraingenh();

        // Update is called once per frame
        void Terraingenh()
        {
            float PerlinNoise3D(float extended, float heughtmulti, float zaxis)
            {
                float xy = Mathf.PerlinNoise(1, 2);
                float xz = Mathf.PerlinNoise(1, 2);
                float yz = Mathf.PerlinNoise(3, 4);
                float yx = Mathf.PerlinNoise(5, 6);
                float zx = Mathf.PerlinNoise(2, 3);
                float zy = Mathf.PerlinNoise(1, 2);

                return (xy + xz + yz + yx + zx + zy) / 6;
            }
        }
    }
}
