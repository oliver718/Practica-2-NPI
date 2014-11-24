using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{

    public struct myPoint
    {
        public double x;
        public double y;
        public double z;
    }

    public class mov19brazoDer
    {
        /// <summary>
        /// valdrá true si comenzó el movimiento de cabeza, false en caso contrario
        /// </summary>
        private bool movIniciado;

        /// <summary>
        /// valdrá true cuando se complete el movimiento de la cabeza hacia la derecha
        /// </summary>
        private bool movArribaIniciado;

        /// <summary>
        /// valdrá true cuando se complete el movimiento de la cabeza hacia la izquierda
        /// </summary>
        private bool movAbajoIniciado;

        /// <summary>
        /// valdrá true cuando el movimiento de cabeza se realice correctamente
        /// </summary>
        private bool movFinalizado;

        Joint hombro, codo, munieca;

        ///<summary>
        /// grado de inclinación
        /// </summary>
        private int gradoActual, gradoPosErguido, gradoDifCodoMunieca, gradoDifHombroCodo,
            gradoActualZ, gradoPosErguidoZ, gradoDif, gradoDifZ, gradoHombroCodo, gradoCodoMunieca, gradoHombroCodoZ, gradoCodoMuniecaZ, gradoDifHombroCodoZ, gradoDifCodoMuniecaZ;


        /// <summary>
        /// Inicializacion de componentes de la clase
        /// </summary>
        public mov19brazoDer()
        {
            movIniciado = movArribaIniciado = movAbajoIniciado = movFinalizado = false;
            gradoActual = gradoPosErguido = gradoActualZ = gradoPosErguidoZ = gradoDif = gradoDifZ = 0;
            gradoHombroCodo = gradoCodoMunieca = gradoHombroCodoZ = gradoCodoMuniecaZ = gradoDifHombroCodoZ = gradoDifCodoMuniecaZ = gradoDifCodoMunieca = gradoDifHombroCodo = 0;
        }

        /// <summary>
        /// Metodo que actualiza los grados de inclinacion de la cabeza
        /// </summary>
        public void actualizarGradosInclinacion(Joint hombro, Joint codo, Joint munieca)
        {
            this.hombro = hombro;
            this.codo = codo;
            this.munieca = munieca;
            gradoHombroCodo = Math.Abs(gradoInclinacion(hombro.Position.X, hombro.Position.Y, codo.Position.X, codo.Position.Y));
            gradoCodoMunieca = calcAngleXY(pointsToVector(hombro,codo), pointsToVector(codo,munieca));
            gradoHombroCodoZ = Math.Abs(gradoInclinacion(hombro.Position.X, hombro.Position.Z, codo.Position.X, codo.Position.Z));
            gradoCodoMuniecaZ = Math.Abs(gradoInclinacion(codo.Position.X, codo.Position.Z, munieca.Position.X, munieca.Position.Z));
        }



        /// <summary>
        /// Método que devuelve true si el movimiento no esta iniciado y si el usuario está preparado para empezar el movimiento
        /// </summary>
        public bool preguntarIniciarMov()
        {
            return !movIniciado && gradoHombroCodo < 10 && gradoCodoMunieca < 5 &&
                gradoHombroCodoZ < 15 && gradoCodoMuniecaZ < 15 && hombro.Position.X < codo.Position.X;
        }

        /// <summary>
        /// Método que que indica al objeto que se ha iniciado el movimiento
        /// </summary>
        public void iniciarMov()
        {
            movIniciado = true;
            movArribaIniciado = true;
        }

        /// <summary>
        /// Método que devuelve true si el movimiento realizado por el usuario es incorrecto
        /// </summary>
        public bool preguntarMovIncorrecto()
        {
            return movIniciado && (gradoHombroCodoZ >= 25 || gradoHombroCodo >= 15 || hombro.Position.X > codo.Position.X ||
                hombro.Position.Y > (munieca.Position.Y + 0.5));
        }

        /// <summary>
        /// Método que que indica al objeto que el movimiento es incorrecto y es necesario reinicar algunos parametros
        /// </summary>
        public void movIncorrecto()
        {
            movIniciado = movArribaIniciado = movAbajoIniciado = false;
        }

        /// <summary>
        /// Método que devuelve true si se ha alcanzado el máximo del movimiento hacia la derecha
        /// </summary>
        public bool preguntarMaxMovArriba()
        {
            return movArribaIniciado && gradoCodoMunieca > 120;
        }

        /// <summary>
        /// Método que que indica al objeto que se ha alcanzado el máximo del movimiento hacia la derecha
        /// </summary>
        public void maxMovArriba()
        {
            movAbajoIniciado = true;
            movArribaIniciado = false;
        }

        /// <summary>
        /// Método que devuelve true si el usuario esta moviendo el cuello hacia la derecha
        /// </summary>
        public bool preguntarMovArriba()
        {
            return movArribaIniciado && gradoCodoMunieca > 10;
        }

        /// <summary>
        /// Método que devuelve true si se ha alcanzado el máximo del movimiento hacia la izquierda
        /// </summary>
        public bool preguntarMaxMovAbajo()
        {
            return movAbajoIniciado && gradoCodoMunieca < 10;
        }

        /// <summary>
        /// Método que que indica al objeto que se ha alcanzado el máximo del movimiento hacia la izquierda
        /// </summary>
        public void maxMovAbajo()
        {
            movAbajoIniciado = false;
            movFinalizado = true;
            movIniciado = false;
        }

        /// <summary>
        /// Método que devuelve true si el usuario esta moviendo el cuello hacia la izquierda
        /// </summary>
        public bool preguntarMovAbajo()
        {
            return movAbajoIniciado && gradoCodoMunieca <= 120;
        }

        /// <summary>
        /// Método que devuelve los grados de inclinación de la cabeza del usuario hacia la derecha o hacia la izquierda
        /// </summary>
        public int getHombroCodo()
        {
            return this.gradoHombroCodo;
        }

        /// <summary>
        /// Método que devuelve los grados de inclinación de la cabeza del usuario hacia adelante ó hacia atras
        /// </summary>
        public int getCodoMunieca()
        {
            return this.gradoCodoMunieca;
        }

        /// <summary>
        /// Método que devuelve los grados de inclinación de la cabeza del usuario hacia la derecha o hacia la izquierda
        /// </summary>
        public int getHombroCodoZ()
        {
            return this.gradoHombroCodoZ;
        }

        /// <summary>
        /// Método que devuelve los grados de inclinación de la cabeza del usuario hacia adelante ó hacia atras
        /// </summary>
        public int getCodoMuniecaZ()
        {
            return this.gradoCodoMuniecaZ;
        }



        /// <summary>
        /// Metodo que devuelve el grado de inclinación de una recta (valor entre 0 y 180).
        /// Entran por parametros las coordenadas de la recta.
        /// </summary>
        private int gradoInclinacion(float x1, float y1, float x2, float y2)
        {
            float grado = 0;
            float m = (y2 - y1) / (x2 - x1);
            grado = (float)(System.Math.Atan(m) * (180 / System.Math.PI));
            //if (grado < 0)
            //    grado = 180 + grado;

            return (int)grado;
        }

        private myPoint pointsToVector(Joint p1, Joint p2)
        {   // This function convert two points to a vector
            myPoint v = new myPoint();
            v.x = p1.Position.X - p2.Position.X;
            v.y = p1.Position.Y - p2.Position.Y;
            v.z = p1.Position.Z - p2.Position.Z;

            return v;
        }

        private int calcAngleXY(myPoint v1, myPoint v2)
        {   // This functions calculates the angle between two vectors.
            double cosin = (v1.x * v2.x) + (v1.y * v2.y);
            double sum1 = Math.Sqrt((v1.x * v1.x) + (v1.y * v1.y));
            double sum2 = Math.Sqrt((v2.x * v2.x) + (v2.y * v2.y));
            cosin = cosin / (sum1 * sum2);
            double angle = Math.Acos(cosin);
            angle = angle * (180 / Math.PI); // To convert to degrees!
            return (int)angle;
        }
    }
    
}
