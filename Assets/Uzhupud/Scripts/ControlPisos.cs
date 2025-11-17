using UnityEngine;

public class ControlPisos : MonoBehaviour
{
    public GameObject[] pisos;
    public MaquetaDetailHandler detalleMaqueta;

    public void CambiarPiso(float valor)
    {
        int indice = Mathf.RoundToInt(valor);

        for (int i = 0; i < pisos.Length; i++)
        {
            pisos[i].SetActive(i <= indice);
            if (i == indice && detalleMaqueta.inTopView) pisos[i].GetComponent<VRSelectable>().Seleccionar();
        }
    }
}
