//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


//dibujar un circulo
//creando primero la variable drawingContext de tipo DrawingContext
//drawingContext.DrawEllipse(inferredJointBrush, null, pos, 6, 6);

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using System;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        /// <summary>
        /// Width of output drawing
        /// </summary>
        // ****Ancho de dibujo de salida
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        // ****Alto de dibujo de salida
        private const float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        // ****Grosor de puntos de detección
        private const double JointThickness = 5;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        private const double BodyCenterThickness = 10;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        // ****Grosor de lineas de borde de pantalla (lineas rojas)
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Brush used to draw skeleton center point
        /// </summary>
        // ****Punto central de detección del cuerpo
        private readonly Brush centerPointBrush = Brushes.Blue;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        // ****color de los puntos de deteccion detectados
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>     
        // ****color de los puntos de deteccion intuidos
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        // ****Grosor y color de huesos detectados de esqueleto
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>   
        // ****Grosor y color de huesos intuidos de esqueleto
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        // ****Activación de kinect
        private KinectSensor sensor;

        /// <summary>
        /// Drawing group for skeleton rendering output
        /// </summary>
        // ****Grupo de dibujo del esqueleto
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        // ****Dibujo de la imagen que se mostrará
        private DrawingImage imageSource;

        //----------------------------------
        //--------variables añadidas--------
        //----------------------------------

        /// <summary>
        /// Para reservar pixels de color
        /// </summary>
        private byte[] colorPixels;

        /// <summary>
        /// mapa de bit de color
        /// </summary>
        private WriteableBitmap colorBitmap;

        /// <summary>
        /// Grosor y color de "hueso" cuello-cabeza
        /// </summary>
        private Pen trackedBonePenHead = new Pen(Brushes.Green, 6);

        /// <summary>
        /// Grosor y color de "hueso" brazo
        /// </summary>
        private Pen trackedBonePenBrazo = new Pen(Brushes.Green, 6);

        /// <summary>
        /// valdrá true si comenzó el movimiento de cabeza, false en caso contrario
        /// </summary>
        private bool movIniciado = false;

        private bool tutorial = false;

        /// <summary>
        /// valdrá true cuando el movimiento de cabeza se realice correctamente
        /// </summary>
        private bool movFinalizado = true;

        ///<summary>
        /// número de frames por segundo
        /// </summary>
        private readonly int FPS = 30;

        ///<summary>
        /// contador de frames
        /// </summary>
        private int contFPS = 0;

        ///<summary>
        /// contador de frames (usado al mostrar la secuencia de imagenes del tutorial)
        /// </summary>
        private int contVideo = 0;

        ///<summary>
        /// contador de imagenes (usado para contar el numero de veces que se muestran las secuencias de imagenes
        /// </summary>
        private int contSecuencia;

        ///<summary>
        /// variable booleana que se utiliza para saber cuando se reproduce la secuencia de imagenes
        /// </summary>
        private bool reproducirSecuencia = false;

        ///<summary>
        /// Primera cadena que se muestra en el tutorial
        /// </summary>
        private string cadenaPosInicial = "Cada repetición tiene que iniciarla con la posición que se muestra en la figura:";

        ///<summary>
        /// segunda cadena que se muestra en el tutorial
        /// </summary>
        private string cadenaTutorial = "A continuación se muestra un secuencia de como tiene que ejecutar cada repetición:";

        ///<summary>
        /// tercera y última cadena que se muestra en el tutorial
        /// </summary>
        private string cadenaEmpezar = "Coloquese en la posición inicial para comenzar su sesión de ejercicio. \n En pantalla se muestran dos puntos amarillos como guia para la posición inicial";

        ///<summary>
        /// variable usada para saber cual es el siguinte caracter que se tiene que mostrar en la escritura de las cadenas del tutorial
        /// </summary>
        private int posEscritura = 0;

        ///<summary>
        /// variable que almacena el número de errores que comete el usuario en un ejercicio
        /// </summary>
        private int contadorErrores = 0;

        ///<summary>
        /// variable que almacena el número de descoordinaciones en los movimientos cabeza/brazo que comete el usuario en un ejercicio
        /// </summary>
        private int contadorDescoordinaciones = 0;

        ///<summary>
        /// numero que ejercicios correctos que lleva el usuario en una sesión de ejercicio
        /// </summary>
        private int movCorrectos = 0;

        ///<summary>
        /// numero que ejercicios incorrectos que lleva el usuario en una sesión de ejercicio
        /// </summary>
        private int movIncorrectos = 0;

        ///<summary>
        /// numero que ejercicios descoordinados que lleva el usuario en una sesión de ejercicio
        /// </summary>
        private int movDescoordinados = 0;

        ///<summary>
        /// variable que almacena el número de veces que el usuario repetirá el ejercicio.
        /// </summary>
        private int contadorRepeticiones = 1;

        ///<summary>
        /// variable usada para mostrar la primera parte del tutorial cuando valga true
        /// </summary>
        private bool tutorialParte1 = false;

        ///<summary>
        /// variable usada para mostrar la segunda parte del tutorial cuando valga true
        /// </summary>
        private bool tutorialParte2 = false;

        ///<summary>
        /// variable usada para mostrar la tercera parte del tutorial cuando valga true
        /// </summary>
        private bool tutorialParte3 = false;

        ///<summary>
        /// variable usada para controla si se dibuja o no el esqueleto
        /// </summary>
        private bool dibujarEsqueleto = false;
        

        /// <summary>
        /// crear objeto mov18cabeza
        /// </summary>
        private mov18cabeza mov18;

        /// <summary>
        /// crear objeto mov24brazoIzq
        /// </summary>
        private mov19brazoDer mov19;


//--------------------------------------------------------------------------------------------------
//------------------------------------------------METODOS-------------------------------------------
//--------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        // ****Inicialización de los componentes de la clase
        public MainWindow()
        {
            InitializeComponent();
            mov18 = new mov18cabeza();
            mov19 = new mov19brazoDer();
            txtAyuda.Text = "Pulse el botón \"Iniciar\" para comenzar con el ejercicio.\nAbajo puede ajustar los parametros que desee.";
            txtRepeticiones.Text = contadorRepeticiones.ToString();
            btnSaltar.Visibility = Visibility.Hidden;
            
        }

        /// <summary>
        /// Draws indicators to show which edges are clipping skeleton data
        /// </summary>
        /// <param name="skeleton">skeleton to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        // ****Bordes en pantalla de continuidad del esqueleto (desactivada su llamada por decisión propia)
        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        // ***Ejecución de tareas iniciales
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            
            // *** Inicializar el sensor
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }


            if (this.sensor != null)
            {
                // En ImageE se proyectará el esqueleto y en ImageC la imagen en color del senson kinect
                // Display the drawing using our image control
                ImageE.Source = this.imageSource;

                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                // color stream
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Espacio reservado para pixels de color
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];

                //Bitmaps para mostrar en pantalla
                this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, 
                    this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Para que en el objeto Image se proyecte la imagen
                this.ImageC.Source = this.colorBitmap;

                this.sensor.ColorFrameReady += this.SensorColorFrameReady;

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }

        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        // ****Ejecución de tareas de apagado
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }


        /// <summary>
        /// Event handler for Kinect sensor's ColorFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        //*** Para poder ver la imagen capturada por la camara kinect
        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    // Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(this.colorPixels);

                    // Write the pixel data into our bitmap
                    this.colorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels,
                        this.colorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
        }

        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        // ****Controlador de eventos del esqueleto
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                //Es necesario que se declare el "color" como transparente para poder ver el esqueleto y la imagen del cuerpo
                dc.DrawRectangle(Brushes.Transparent, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                

                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {                      
                        //RenderClippedEdges(skel, dc);  //Desactivada la llamada a esta función por decisión propia

                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            if (tutorial)
                                iniciarTutorial();//lanzar el tutorial
                            else
                            {
                                if (!movFinalizado)
                                    this.movimiento18y19(skel); //llamada al metodo para detectar los movimientos
                                this.DrawBonesAndJoints(skel, dc);
                                
                            }
                        }
                        else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                        {
                            dc.DrawEllipse(
                            this.centerPointBrush,
                            null,
                            this.SkeletonPointToScreen(skel.Position),
                            BodyCenterThickness,
                            BodyCenterThickness);
                           
                        }
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }


        /// <summary>
        /// Metodo principal para controlar la secuencia de movimintos de los ejercicios
        /// </summary>
        /// <param name="skeleton">objeto esqueleto</param>
        private void movimiento18y19(Skeleton skeleton)
        {
            Joint cabeza = skeleton.Joints[JointType.Head];
            Joint centroPecho = skeleton.Joints[JointType.ShoulderCenter];
            Joint hombro = skeleton.Joints[JointType.ShoulderRight];
            Joint codo = skeleton.Joints[JointType.ElbowRight];
            Joint munieca = skeleton.Joints[JointType.WristRight];
            

            if (contFPS == 0)//Cada tercio de segundo se actualizarán los grados de inclinación de las partes del cuerpo involucradas en los movimientos
            {
                mov18.actualizarGradosInclinacion(cabeza, centroPecho);
                mov19.actualizarGradosInclinacion(hombro, codo, munieca);
            }

            contFPS++;
            if(contFPS == FPS/3)
                contFPS = 0;

            //Si el movimiento no se ha iniciado y el usuario se encuentra en las posiciones correctas el movimiento se iniciará
            if (mov18.preguntarIniciarMov() && mov19.preguntarIniciarMov())
            {
                mov18.iniciarMov();
                mov19.iniciarMov();
                movIniciado = true;
                trackedBonePenHead.Brush = Brushes.Yellow;
                trackedBonePenBrazo.Brush = Brushes.Yellow;
                txtAyuda.Text = "Ejercicios correctos: " + movCorrectos.ToString() + "\nEjercicios descordinados: " + movDescoordinados.ToString() +
                    "\nEjercicios erroneos: " + movIncorrectos.ToString();
                contadorErrores = 0;
                contadorDescoordinaciones = 0;
            }
            //Controla si el usuario ha cometido algún error en algunos de los movimientos
            else if (mov18.preguntarMovIncorrecto() || mov19.preguntarMovIncorrecto())
            {
                contadorErrores++;
                if(mov18.preguntarMovIncorrecto())
                    trackedBonePenHead.Brush = Brushes.Red;
                if(mov19.preguntarMovIncorrecto())
                    trackedBonePenBrazo.Brush = Brushes.Red;
            }
            //Controla cuando el usuario completa la primera parte de del ejercicio (brazo hacia arriba y cabeza hacia la derecha)
            else if (mov18.preguntarMaxMovDerecha() || mov19.preguntarMaxMovArriba())
            {
                if (!mov18.preguntarMaxMovDerecha() || !mov19.preguntarMaxMovArriba())
                    contadorDescoordinaciones++;
                if (mov18.preguntarMaxMovDerecha())
                {
                    mov18.maxMovDerecha();
                    trackedBonePenHead.Brush = Brushes.Aqua;
                }
                if (mov19.preguntarMaxMovArriba())
                {
                    mov19.maxMovArriba();
                    trackedBonePenBrazo.Brush = Brushes.Aqua;
                }
            }
            //Controla cuando se completa la segunda parte del movimiento (brazo hacia abajo y cabeza hacia la izquierda)
            else if (mov18.preguntarMaxMovIzquierda() || mov19.preguntarMaxMovAbajo())
            {
                if (!mov18.preguntarMaxMovIzquierda() || !mov19.preguntarMaxMovAbajo())
                    contadorDescoordinaciones++;
                if (mov18.preguntarMaxMovIzquierda())
                {
                    mov18.maxMovIzquierda();
                    trackedBonePenHead.Brush = Brushes.Green;
                }
                if (mov19.preguntarMaxMovAbajo())
                {
                    mov19.maxMovAbajo();
                    trackedBonePenBrazo.Brush = Brushes.Green;
                }
                if (mov18.getFinalizado() && mov19.getFinalizado())//cuando finalizan las dos partes
                {
                    contadorRepeticiones--;
                    movIniciado = false;
                    actualizarMarcadores();
                    txtAyuda.Text = "Ejercicios correctos: " + movCorrectos.ToString() + "\nEjercicios descordinados: " + movDescoordinados.ToString() +
                    "\nEjercicios erroneos: " + movIncorrectos.ToString();
                    if (contadorRepeticiones == 0)
                    {
                        lblFin.Visibility = Visibility.Visible;
                        movFinalizado = true;
                        movCorrectos = 0;
                        movDescoordinados = 0;
                        movIncorrectos = 0;
                    }
                }
            }
            //controla si el usuario ha realizado mal el movimiento y lo finaliza saltandose algún paso.
            else if (mov18.preguntarMovMalFinalizado())
            {
                mov18.maxMovDerecha();
                mov19.maxMovArriba();
                contadorErrores = 100;
            }
        }


        /// <summary>
        /// Método que actualiza los marcadores del  usuario en una sesión de ejercicio (correctos, incorrectos y descoordinados)
        /// </summary>
        private void actualizarMarcadores()
        {
            if (contadorErrores > 10)
                movIncorrectos++;
            else if (contadorDescoordinaciones == 4)
                movDescoordinados++;
            else
                movCorrectos++;
        }
        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        // ***Dibujar los huesos y articulaciones del esqueleto
        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (dibujarEsqueleto)
            {
                // Render Torso
                this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
                this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
                this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
                this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
                this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
                this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
                this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

                // Left Arm
                this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
                this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
                this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

                // Right Arm
                this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
                this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
                this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

                // Left Leg
                this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
                this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
                this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

                // Right Leg
                this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
                this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
                this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);



                // Render Joints
                foreach (Joint joint in skeleton.Joints)
                {
                    Brush drawBrush = null;

                    if (joint.TrackingState == JointTrackingState.Tracked)
                    {
                        drawBrush = this.trackedJointBrush;
                    }
                    else if (joint.TrackingState == JointTrackingState.Inferred)
                    {
                        drawBrush = this.inferredJointBrush;
                    }

                    if (drawBrush != null)
                    {
                        drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);


                    }
                }
            }

            if (!movIniciado && !movFinalizado) //dibujar los puntos de guia para la posición inicial
            {
                SkeletonPoint posSkeleton = new SkeletonPoint();
                posSkeleton.X = skeleton.Joints[JointType.ShoulderCenter].Position.X;
                posSkeleton.Y = skeleton.Joints[JointType.Head].Position.Y;
                posSkeleton.Z = skeleton.Joints[JointType.ShoulderCenter].Position.Z;
                Point pos = this.SkeletonPointToScreen(posSkeleton);
                drawingContext.DrawEllipse(inferredJointBrush, null, pos, 10, 10);
                posSkeleton.X = (float)getXPuntoMano(skeleton.Joints[JointType.ShoulderRight].Position.X, skeleton.Joints[JointType.ShoulderLeft].Position.X);
                posSkeleton.Y = skeleton.Joints[JointType.ShoulderRight].Position.Y;
                posSkeleton.Z = skeleton.Joints[JointType.ShoulderRight].Position.Z;
                pos = this.SkeletonPointToScreen(posSkeleton);
                drawingContext.DrawEllipse(inferredJointBrush, null, pos, 10, 10);
            }
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        /// <summary>
        /// Draws a bone line between two joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param>
        /// <param name="jointType1">joint to end drawing at</param>
        // ****Dibujar una linea de hueso entre dos articulaciones
        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                //el "hueso" cabeza-hombro lo pintará del color deseado según si el movimiento es correcto o no
                if (joint0 == skeleton.Joints[JointType.Head]) 
                    drawPen = this.trackedBonePenHead;
                else if (joint0 == skeleton.Joints[JointType.ShoulderRight] || joint0 == skeleton.Joints[JointType.ElbowRight] || joint0 == skeleton.Joints[JointType.WristRight])
                    drawPen = this.trackedBonePenBrazo;
                else
                    drawPen = this.trackedBonePen;
            }
            
            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));            
        }

        /// <summary>
        /// Handles the checking or unchecking of the seated mode combo box
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        // ****Activar o desactivar el modo sentado
        private void CheckBoxSeatedModeChanged(object sender, RoutedEventArgs e)
        {
            if (null != this.sensor)
            {
                if (this.checkBoxSeatedMode.IsChecked.GetValueOrDefault())
                {
                    this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                }
                else
                {
                    this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
                }
            }
        }


        /// <summary>
        /// Método que lanza el tutorial del movimiento, el cual se divide en tres partes.
        /// </summary>
        private void iniciarTutorial()
        {
            if (tutorialParte1 && contFPS == 0)
            {
                if (posEscritura == 0)
                    txtAyuda.Text = "";
                ImageC.Visibility = Visibility.Hidden;
                ImageE.Visibility = Visibility.Hidden;
                figura1.Visibility = Visibility.Visible;
                txtAyuda.Text += cadenaPosInicial[posEscritura].ToString();
                posEscritura++;
                if (posEscritura == cadenaPosInicial.Length)
                {
                    tutorialParte1 = false;
                    tutorialParte2 = true;
                    contFPS = -(FPS*2); //espera para que el usuario vea la pos inicial
                    posEscritura = 0;
                }
            }

            else if (tutorialParte2)
            {
                if (contFPS == 0 && !reproducirSecuencia)
                {
                    if (txtAyuda.Text == cadenaPosInicial)
                        txtAyuda.Text = "";
                    txtAyuda.Text += cadenaTutorial[posEscritura].ToString();
                    posEscritura++;
                    if (posEscritura == cadenaTutorial.Length)
                        reproducirSecuencia = true;
                    else if (posEscritura == cadenaTutorial.Length)
                        cadenaTutorial += " ";
                }
                if (reproducirSecuencia && contVideo == 0)
                {
                    if (figura1.Visibility == Visibility.Visible)//secuencia de imagenes
                    {
                        figura1.Visibility = Visibility.Hidden;
                        figura2.Visibility = Visibility.Visible;
                    }
                    else if (figura2.Visibility == Visibility.Visible)
                    {
                        figura2.Visibility = Visibility.Hidden;
                        figura3.Visibility = Visibility.Visible;
                    }
                    else if (figura3.Visibility == Visibility.Visible)
                    {
                        figura3.Visibility = Visibility.Hidden;
                        figura4.Visibility = Visibility.Visible;
                    }
                    else if (figura4.Visibility == Visibility.Visible)
                    {
                        figura4.Visibility = Visibility.Hidden;
                        figura5.Visibility = Visibility.Visible;
                    }
                    else if (figura5.Visibility == Visibility.Visible)
                    {
                        figura5.Visibility = Visibility.Hidden;
                        figura1.Visibility = Visibility.Visible;
                    }
                    contSecuencia++;
                    if (contSecuencia == 25)
                    {
                        tutorialParte2 = false;
                        tutorialParte3 = true;
                        posEscritura = 0;
                        contSecuencia = 0;
                        contFPS = -(FPS * 2);
                    }
                }
                contVideo++;
                if (contVideo == FPS / 2)
                    contVideo = 0;
            }
            else if (tutorialParte3 && contFPS == 0)
            {
                ImageC.Visibility = Visibility.Visible;
                ImageE.Visibility = Visibility.Visible;
                figura1.Visibility = Visibility.Hidden;
                figura2.Visibility = Visibility.Hidden;
                figura3.Visibility = Visibility.Hidden;
                figura4.Visibility = Visibility.Hidden;
                figura5.Visibility = Visibility.Hidden;
                if (txtAyuda.Text == cadenaTutorial)
                    txtAyuda.Text = "";
                txtAyuda.Text += cadenaEmpezar[posEscritura].ToString();
                posEscritura++;
                if (posEscritura == cadenaEmpezar.Length)
                {
                    tutorialParte3 = false;
                    tutorial = false;
                    posEscritura = 0;
                    btnSaltar.Visibility = Visibility.Hidden;
                }
            }

            contFPS++;
            if (contFPS == FPS / 10)
                contFPS = 0;
        }

        /// <summary>
        /// Método que salta cuando se pulsa el botón de iniciar ejercicio
        /// </summary>
        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            tutorial = true;
            tutorialParte1 = true;
            txtAyuda.Text = "Coloquese frente a kinect para comenzar con el tutorial...";
            movCorrectos = 0;
            movIncorrectos = 0;
            movDescoordinados = 0;
            contFPS = 0;
            movFinalizado = false;
            lblFin.Visibility = Visibility.Hidden;
            btnSaltar.Visibility = Visibility.Visible;
            contadorRepeticiones = int.Parse(txtRepeticiones.Text);
            mov18 = new mov18cabeza();
            mov19 = new mov19brazoDer();


        }

        /// <summary>
        /// Método que salta cuando se pulsa el botón para establecer el número de repeticiones
        /// </summary>
        private void btnEstablecer_Click(object sender, RoutedEventArgs e)
        {
            int repe = int.Parse(txtRepeticiones.Text);
            if (repe > 1 && repe <= 100)
                contadorRepeticiones = repe;
            txtRepeticiones.Text = contadorRepeticiones.ToString();
        }

        /// <summary>
        /// Método que se ejecuta al pulsar el botón de saltar el tutorial
        /// </summary>
        private void btnSaltar_Click(object sender, RoutedEventArgs e)
        {
            ImageC.Visibility = Visibility.Visible;
            ImageE.Visibility = Visibility.Visible;
            figura1.Visibility = Visibility.Hidden;
            figura2.Visibility = Visibility.Hidden;
            figura3.Visibility = Visibility.Hidden;
            figura4.Visibility = Visibility.Hidden;
            figura5.Visibility = Visibility.Hidden;
            tutorial = false;
            tutorialParte1 = false;
            tutorialParte2 = false;
            tutorialParte3 = false;
            txtAyuda.Text = "";
            btnSaltar.Visibility = Visibility.Hidden;
            posEscritura = 0;
            
        }

        /// <summary>
        /// Método que devuelve el punto donde hay que pintar el circulo guia para que el usuario ponga la mano en la posición inicial.
        /// Se toma como referencia la distancia entre los hombros * 2
        /// </summary>
        private double getXPuntoMano(double hombroR, double hombroL)
        {
            double dis;
            if (hombroR > 0 && hombroL > 0 || hombroR < 0 && hombroL < 0)
                dis = Math.Abs(Math.Abs(hombroR) - Math.Abs(hombroL));
            else
                dis = Math.Abs(hombroR) + Math.Abs(hombroL);

            return hombroR + (dis * 2);
        }

        /// <summary>
        /// Método del checkbox para activar o desactivar el dibujar el esqueleto
        /// </summary>
        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            if (!dibujarEsqueleto)
                dibujarEsqueleto = true;
            else
                dibujarEsqueleto = false;

        }
    }
}



