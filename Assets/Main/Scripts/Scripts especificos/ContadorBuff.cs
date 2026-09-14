using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Muestra una fila por cada buff activo (ej. "Velocidad: 2.3s").
/// Requiere: un contenedor (Transform) con un Vertical Layout Group,
/// y un prefab de fila con un componente Text en algún hijo.
/// </summary>
public class ContadorBuff : MonoBehaviour
{
    [SerializeField] private ManagerBuff buffManager;
    [SerializeField] private Transform buffListParent;
    [SerializeField] private GameObject buffRowPrefab;

    private readonly Dictionary<string, GameObject> activeRows = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        buffManager.OnBuffsChanged += RebuildRows;
    }

    private void OnDisable()
    {
        buffManager.OnBuffsChanged -= RebuildRows;
    }

    private void Update()
    {
        // Refresca el texto de tiempo restante cada frame
        foreach (ManagerBuff.ActiveBuff buff in buffManager.ActiveBuffs)
        {
            if (activeRows.TryGetValue(buff.Name, out GameObject row))
            {
                Text label = row.GetComponentInChildren<Text>();
                if (label != null)
                {
                    label.text = $"{buff.Name}: {buff.TimeRemaining:F1}s";
                }
            }
        }
    }

    private void RebuildRows()
    {
        // Elimina filas de buffs que ya terminaron
        List<string> toRemove = activeRows.Keys
            .Where(key => !buffManager.ActiveBuffs.Any(b => b.Name == key))
            .ToList();

        foreach (string key in toRemove)
        {
            Destroy(activeRows[key]);
            activeRows.Remove(key);
        }

        // Crea filas para buffs nuevos
        foreach (ManagerBuff.ActiveBuff buff in buffManager.ActiveBuffs)
        {
            if (!activeRows.ContainsKey(buff.Name))
            {
                GameObject row = Instantiate(buffRowPrefab, buffListParent);
                activeRows.Add(buff.Name, row);
            }
        }
    }
}