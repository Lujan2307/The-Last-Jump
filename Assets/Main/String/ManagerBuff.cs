using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Registro central de los buffs temporales activos en el jugador
/// (Velocidad, Daño, etc.), pensado para alimentar la UI.
/// Los efectos reales los siguen aplicando PlayerMovement / PlayerDamage;
/// este manager NO los aplica, solo lleva la cuenta del tiempo restante.
/// </summary>
public class ManagerBuff : MonoBehaviour
{
    [Serializable]
    public class ActiveBuff
    {
        public string Name;
        public float Duration;
        public float TimeRemaining;
    }

    private readonly List<ActiveBuff> activeBuffs = new List<ActiveBuff>();
    public List<ActiveBuff> ActiveBuffs => activeBuffs;

    // Se dispara solo cuando un buff se agrega o se termina (no cada frame),
    // para que la UI sepa cuándo crear/destruir filas.
    public event Action OnBuffsChanged;

    public void AddBuff(string buffName, float duration)
    {
        ActiveBuff existing = activeBuffs.Find(b => b.Name == buffName);

        if (existing != null)
        {
            // Ya está activo: reinicia la duración en vez de acumular
            existing.Duration = duration;
            existing.TimeRemaining = duration;
        }
        else
        {
            activeBuffs.Add(new ActiveBuff
            {
                Name = buffName,
                Duration = duration,
                TimeRemaining = duration
            });
        }

        OnBuffsChanged?.Invoke();
    }

    private void Update()
    {
        if (activeBuffs.Count == 0) return;

        bool removedAny = false;

        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].TimeRemaining -= Time.deltaTime;

            if (activeBuffs[i].TimeRemaining <= 0f)
            {
                activeBuffs.RemoveAt(i);
                removedAny = true;
            }
        }

        if (removedAny)
        {
            OnBuffsChanged?.Invoke();
        }
    }
}