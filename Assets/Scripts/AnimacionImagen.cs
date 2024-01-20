using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionImagen : MonoBehaviour
{
    public float escalaMaxima = 1.2f; // Escala máxima que alcanzará la imagen
    public float duracionAnimacion = 3f; // Duración total de la animación en segundos

    private void Start()
    {
        // Llama a la función para iniciar la animación cuando comienza el juego
        IniciarAnimacion();
    }

    private void IniciarAnimacion()
    {
        // Escala inicial de la imagen
        Vector3 escalaInicial = transform.localScale;

        // Escala final de la imagen (escala inicial * escala máxima)
        Vector3 escalaFinal = escalaInicial * escalaMaxima;

        // Crea una secuencia de animación usando LeanTween
        LeanTween.scale(gameObject, escalaFinal, duracionAnimacion / 2).setEaseOutQuad()
            .setOnComplete(() =>
            {
                // Al completar la primera animación, realiza la segunda animación
                LeanTween.scale(gameObject, escalaInicial, duracionAnimacion / 2).setEaseInQuad()
                    .setOnComplete(() =>
                    {
                        // Después de completar la segunda animación, reinicia la secuencia
                        IniciarAnimacion();
                    });
            });
    }
}


