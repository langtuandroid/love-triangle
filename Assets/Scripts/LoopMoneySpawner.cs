using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class LoopMoneySpawner : MonoBehaviour {
    public GameObject moneyToInstantiate;

    public SplinePositioner startingMoney;
    public int setCount;
    public int count;
    public float spacing;
    public float direction;

    [ContextMenu("InstantiateLoopMoney_1")]
    public void InstantiateLoopMoney_1() {
        for (int a = 0; a < setCount; ++a) {

            SplinePositioner sp;
            for (int x = 1; x < count + 1; ++x) {
                GameObject go = Instantiate(moneyToInstantiate);
                sp = go.AddComponent<SplinePositioner>();
                sp.spline = startingMoney.spline;
                go.transform.SetParent(transform);
                sp.SetPercent(startingMoney.GetPercent() - (spacing * x));
                sp.motion.offset = new Vector2((startingMoney.motion.offset.x - (direction * x)), startingMoney.motion.offset.y);
                if (x + 1 >= count + 1) {
                    startingMoney = sp;
                    direction *= -1f;
                }
            }
        }
        
    }

    [ContextMenu("InstantiateLoopMoney_2")]
    public void InstantiateLoopMoney_2() {
        direction = startingMoney.motion.offset.x;
        SplinePositioner sp;
        for (int a = 0; a < setCount; ++a) {

            
            
            for (int x = 1; x < count + 1; ++x) {
                GameObject go = Instantiate(moneyToInstantiate);
                sp = go.AddComponent<SplinePositioner>();
                sp.spline = startingMoney.spline;
                go.transform.SetParent(transform);
                sp.SetPercent(startingMoney.GetPercent() - (spacing * x));
                sp.motion.offset = new Vector2((direction), startingMoney.motion.offset.y);
                if (x + 1 >= count + 1) {
                    startingMoney = sp;
                    direction *= -1f;
                }
            }
        }
    }

    [ContextMenu("InstantiateLoopMoney_3")]
    public void InstantiateLoopMoney_3() {
        direction = startingMoney.motion.offset.x;
        float spaceFactor = 0.008f;
        SplinePositioner sp;
        for (int a = 0; a < setCount; ++a) {
            for (int x = 0; x < count; ++x) {
                GameObject go = Instantiate(moneyToInstantiate);
                sp = go.AddComponent<SplinePositioner>();
                sp.spline = startingMoney.spline;
                go.transform.SetParent(transform);
                if (x % 2 == 0) {
                    sp.motion.offset = new Vector2((direction + 1), startingMoney.motion.offset.y);
                    sp.SetPercent((startingMoney.GetPercent() - (spacing * (x / 2))) - spaceFactor);
                }
                else {
                    sp.motion.offset = new Vector2((direction), startingMoney.motion.offset.y);
                    sp.SetPercent((startingMoney.GetPercent() - (spacing * (x / 2))) - spaceFactor);
                }
                if (x + 1 >= count) {
                    startingMoney = sp;
                    direction *= -1f;
                }
            }
        }
    }
    [ContextMenu("InstantiateLoopMoney_4")]
    public void InstantiateLoopMoney_4() {
        direction = startingMoney.motion.offset.x;
        float spaceFactor = 0f;
        SplinePositioner sp;
        
        for (int a = 0; a < setCount; ++a) {
            int y = 0;
            for (int x = 0; x < 15; ++x) {
                GameObject go = Instantiate(moneyToInstantiate);
                sp = go.AddComponent<SplinePositioner>();
                sp.spline = startingMoney.spline;
                go.transform.SetParent(transform);
                sp.SetPercent((startingMoney.GetPercent() - (spacing * y)) - spaceFactor);
                if (x % 3 == 0) {
                    sp.motion.offset = new Vector2(-3f, startingMoney.motion.offset.y);
                    
                } else if (x % 3 == 1) {
                    sp.motion.offset = new Vector2(0f, startingMoney.motion.offset.y);

                }
                else {
                    sp.motion.offset = new Vector2(3f, startingMoney.motion.offset.y);
                    y++;
                }
                
                if (x + 1 >= 15) {
                    startingMoney = sp;
                    spaceFactor = 0.035f;
                    //direction *= -1f;
                }
            }
        }
    }
}
