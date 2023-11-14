using Grapple;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesLogic : MonoBehaviour {

    public PlayerController infoMovimiento;
    public GrapplingHookLogic infoGancho;
    public FishColor infoPeces;
    public int costo;
    public int upgradeLevel;
    public int tipoMejora;
    public int valorMejora;
    private bool purchased;


    // Start is called before the first frame update
    void Start(){
        purchased = false;
    }

    public void Comprar()
    {
        int money = infoGancho.GetMoney();
        if (!purchased && money < costo) {
            infoGancho.SetMoney(money-costo);
        }
    }

    public void AplicarMejora() {
        switch(tipoMejora)
        {
            case 0:
                infoMovimiento.SetSpeed(infoMovimiento.GetSpeed() + valorMejora);
                break;
            case 1:
                infoGancho.SetAlcance(infoGancho.GetAlcance() + valorMejora);
                break;
            default:

                break;
        }
    }

}
