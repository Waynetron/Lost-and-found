using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour {
    public GameObject gridDotPrefab;

    float terrainWidth = 1500;
    float terrainHeight = 1200;
    int width = 18;
    int height = 14;
    int buffer = 10;

    void Start() {
        float spacingX = terrainWidth / width;
        float spacingY = terrainHeight / height;

        for (int z = -buffer; z < height + 1 + buffer; z++) {
            for (int x = -buffer; x < width + 1 + buffer; x++) {
                var gridDot = Instantiate(gridDotPrefab);
                gridDot.transform.parent = transform;
                gridDot.transform.localPosition = new Vector3(
                    x * spacingX,
                    150,
                    z * spacingY
                );
            }
        }
    }
}