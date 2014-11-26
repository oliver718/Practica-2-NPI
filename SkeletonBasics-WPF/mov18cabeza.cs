using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    public class mov18cabeza
    {
        /// <summary>
        /// valdrá true si comenzó el movimiento de cabeza, false en caso contrario
        /// </summary>
        private bool movIniciado;

        /// <summary>
        /// valdrá true cuando se complete el movimiento de la cabeza hacia la derecha
        /// </summary>
        private bool movDerechaIniciado;

        /// <summary>
        /// valdrá true cuando se complete el movimiento de la cabeza hacia la izquierda
        /// </summary>
        private bool movIzquierdaIniciado;

        /// <summary>
        /// valdrá true cuando el movimiento de cabeza se realice correctamente
        /// </summary>
        private bool movFinalizado;

        ///<summary>
        /// grado de inclinación
        /// </summary>
        private int gradoActual, gradoPosErguido,
            gradoActualZ, gradoPosErguidoZ, gradoDif, gradoDifZ;

        private int rangoErrorX = 5;

        private int rangoErrorZ = 15;

        private int rangoMinimoMovX = 25; 


        /// <summary>
        /// Inicializacion de componentes de la clase
        /// </summary>
        public mov18cabeza()
        {
            movIniciado = movDerechaIniciado = movIzquierdaIniciado = movFinalizado = false;
            gradoActual = gradoPosErguido = gradoActualZ = gradoPosErguidoZ = gradoDif = gradoDifZ = 0;
        }

        /// <summary>
        /// Metodo que actualiza los grados de inclinacion de la cabeza
        /// </summary>
        public void actualizarGradosInclinacion(Joint cabeza, Joint centroPecho)
        {
            try
            {
                gradoPosErguido = gradoInclinacion(centroPecho.Position.X, centroPecho.Position.Y, centroPecho.Position.X, cabeza.Position.Y);
                gradoPosErguidoZ = gradoInclinacion(centroPecho.Position.Z, centroPecho.Position.Y, centroPecho.Position.Z, cabeza.Position.Y);
                gradoActual = gradoInclinacion(centroPecho.Position.X, centroPecho.Position.Y, cabeza.Position.X, cabeza.Position.Y);
                gradoActualZ = gradoInclinacion(centroPecho.Position.Z, centroPecho.Position.Y, cabeza.Position.Z, cabeza.Position.Y);
                gradoDif = gradoPosErguido - gradoActual;
                gradoDifZ = System.Math.Abs(gradoPosErguidoZ - gradoActualZ); //se suma 10 para regular el error de kinect en la detección
            }
            catch { }
        }


        /// <summary>
        /// Método que devuelve true si el movimiento no esta iniciado y si el usuario está preparado para empezar el movimiento
        /// </summary>
        public bool preguntarIniciarMov()
        {
            return !movIniciado && Math.Abs(gradoDif) < rangoErrorX && gradoDifZ <= rangoErrorZ;
        }

        /// <summary>
        /// Método que que indica al objeto que se ha iniciado el movimiento
        /// </summary>
        public void iniciarMov()
        {
            movIniciado = true;
            movDerechaIniciado = true;
        }

        /// <summary>
        /// Método que devuelve true si el movimiento realizado por el usuario es incorrecto
        /// </summary>
        public bool preguntarMovIncorrecto()
        {
            return movIniciado && gradoDifZ > rangoErrorZ;
        }

        /// <summary>
        /// Método que que indica al objeto que el movimiento es incorrecto y es necesario reinicar algunos parametros
        /// </summary>
        public void movIncorrecto()
        {
            movIniciado = movDerechaIniciado = movIzquierdaIniciado = false;
        }

        /// <summary>
        /// Método que devuelve true si se ha alcanzado el máximo del movimiento hacia la derecha
        /// </summary>
        public bool preguntarMaxMovDerecha()
        {
            return movDerechaIniciado && gradoDif >= rangoMinimoMovX;
        }

        /// <summary>
        /// Método que que indica al objeto que se ha alcanzado el máximo del movimiento hacia la derecha
        /// </summary>
        public void maxMovDerecha()
        {
            movIzquierdaIniciado = true;
            movDerechaIniciado = false;
        }

        /// <summary>
        /// Método que devuelve true si se ha alcanzado el máximo del movimiento hacia la izquierda
        /// </summary>
        public bool preguntarMaxMovIzquierda()
        {
            return movIzquierdaIniciado && gradoDif <= -rangoMinimoMovX;
        }

        /// <summary>
        /// Método que que indica al objeto que se ha alcanzado el máximo del movimiento hacia la izquierda
        /// </summary>
        public void maxMovIzquierda()
        {
            movIzquierdaIniciado = false;
            movFinalizado = true;
            movIniciado = false;
        }

        public bool getFinalizado()
        {
            return movFinalizado;
        }

        /// <summary>
        /// Método que devuelve los grados de inclinación de la cabeza del usuario hacia la derecha o hacia la izquierda
        /// </summary>
        public int getGradosInclinacionX()
        {
            return this.gradoDif;
        }

        /// <summary>
        /// Método que devuelve los grados de inclinación de la cabeza del usuario hacia adelante ó hacia atras
        /// </summary>
        public int getGradosInclinacionZ()
        {
            return this.gradoDifZ;
        }



        /// <summary>
        /// Metodo que devuelve el grado de inclinación de una recta (valor entre 0 y 180).
        /// Entran por parametros las coordenadas de la recta.
        /// </summary>
        private int gradoInclinacion(float x1, float y1, float x2, float y2)
        {
            //grados devueltos segun sea la recta: (el valor devuelto oscila entre 0 y 180 grados
            //----------90 grados
            //           |
            //180grados__.__ 0 grados
            //           |
            //          90 grados

            float grado = 0;
            float m = (y2 - y1) / (x2 - x1);
            grado = (float)(System.Math.Atan(m) * (180 / System.Math.PI));
            if (grado < 0)
                grado = 180 + grado;

            return (int)grado;
        }
    }
}
