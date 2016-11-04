using Assets.Script.NyshaRig.Excersice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    public class RigExercisePlayer:MonoBehaviour,IExercisePlayer
    {

        public string Editor_ExerciseName = string.Empty;

        public int Editor_MaxRounds = 1;
        public int Editor_CurrentRound;

        public bool Editor_LoadExercise = false;
        public bool Editor_StartExercise = false;
        public bool Editor_PauseExercise = false;
        public bool Editor_StopExercise = false;
        public bool Editor_PrepareExercise = false;
        public bool Editor_GoToRest = false;



        private RigPose RestPose;
        private RigAnimation RigAnim;

        private RigSetup Rig;
        private RigPose CurrentPose;
        private float totalDeltaTime = 0;
        private eTransitionState currentTransitionState;

        public ExerciseInfo exerInfo;
        [Space(5)]

        public bool AlternateMirroring = true;

        public bool IsAnimPlaying = false;
        public bool IsAnimExercisePreSetup = false;
        public bool IsExerciseInitialized = false;
        public bool MirrorAnim = false;

        public bool LoadExercise(string name)
        {
            RigAnim = RigAnimation.LoadFromFile(name);
            RestPose = RigPose.LoadFromFile(string.Format("rest_{0}", name));

            CurrentPose = RestPose;
            IsExerciseInitialized = false;

            return (RigAnim != null && RestPose != null);
        }

        public void PlayAnimation()
        {
            IsAnimPlaying = true;
        }

        public void PlayRestPose()
        {
            Rig.SetFromPose(RestPose);
        }

        public void PauseAnimation()
        {
            IsAnimPlaying = false;
        }

        public void ResumeAnimation()
        {
            IsAnimPlaying = true;
        }

        public void StopAnimation()
        {
            IsAnimPlaying = false;
        }

        public void RestartAnimation()
        {
            StopAnimation();
            PlayAnimation();
        }

        public void Start()
        {
            Rig = GetComponentInParent<RigSetup>();
            currentTransitionState = eTransitionState.OnWarmUp;
        }

        public void Update()
        {
            if (Editor_LoadExercise)
            {
                Editor_LoadExercise = false;
                LoadExercise(Editor_ExerciseName);
                PlayRestPose();
            }
            if (Editor_StartExercise)
            {
                Editor_StartExercise = false;
                if (IsExerciseInitialized)
                    IsAnimPlaying = true;
            }
            if (Editor_PauseExercise)
            { }
            if (Editor_StopExercise)
            { }
            if (Editor_PrepareExercise)
            {
                Editor_PrepareExercise = false;
                InitializeExercise(new Exercise(Movement.EstocadaFrontalLarga, Limb.Interleaved), new BehaviourParams(360, 1.1f, 0.9f, 2, 3));
            }
            if (Editor_GoToRest)
            {
                StopAnimation();
                PlayRestPose();
            }



            if (IsAnimPlaying)
            {
                Rig.SetFromPose(CurrentPose);
                //ActivePoseName = CurrentPose.PoseName;


                if (exerInfo.TransitionAlpha >= exerInfo.TransitionThreshold && currentTransitionState == eTransitionState.OnExtremeToRest)
                {
                    Debug.Log("ReachRest");
                    exerInfo.TransitionAlpha = 0;
                    totalDeltaTime = 0;
                    currentTransitionState = eTransitionState.OnRest;
                    if (AlternateMirroring)
                        MirrorAnim = !MirrorAnim;
                    if (IsAnimExercisePreSetup)
                    {
                        IsAnimExercisePreSetup = false;
                        IsExerciseInitialized = true;
                        StopAnimation();
                        if (OnInitializeExerciseEnd != null)
                            OnInitializeExerciseEnd.Invoke(this, new PrepareEventArgs(PrepareStatus.Prepared, Caller.Preview));
                    }


                }
                else if (exerInfo.TransitionAlpha >= exerInfo.TransitionThreshold && currentTransitionState == eTransitionState.OnRestToExtreme)
                {
                    Debug.Log("ReachExtreme");
                    exerInfo.TransitionAlpha = 0;
                    totalDeltaTime = 0;
                    currentTransitionState = eTransitionState.OnExtreme;

                   
                }

                switch (currentTransitionState)
                {
                    case eTransitionState.None:
                        break;
                    case eTransitionState.OnWarmUp:
                        totalDeltaTime += Time.deltaTime;
                        if (totalDeltaTime >= exerInfo.WaitTimeWarmUp / exerInfo.SpeedModifier)
                        {
                            totalDeltaTime = 0;
                            currentTransitionState = eTransitionState.OnRestToExtreme;
                        }
                        break;
                    case eTransitionState.OnRest:
                        //Debug.Log("OnRest");
                        totalDeltaTime += Time.deltaTime;
                        if (totalDeltaTime >= exerInfo.WaitTimeOnRest / exerInfo.SpeedModifier)
                        {
                            totalDeltaTime = 0;
                            currentTransitionState = eTransitionState.OnRestToExtreme;
                        }
                        break;
                    case eTransitionState.OnRestToExtreme:
                        totalDeltaTime += Time.deltaTime;
                        exerInfo.TransitionAlpha = totalDeltaTime / exerInfo.TransitionTimeRestToExtreme * exerInfo.SpeedModifier;
                        if (MirrorAnim)
                            CurrentPose = RigAnim.GetFinalPoseAtAnimPercentage(exerInfo.TransitionAlpha).GetMirroredPose();
                        else
                            CurrentPose = RigAnim.GetFinalPoseAtAnimPercentage(exerInfo.TransitionAlpha);
                        break;
                    case eTransitionState.OnExtreme:
                        //Debug.Log("OnExtreme");
                        totalDeltaTime += Time.deltaTime;
                        if (totalDeltaTime >= exerInfo.WaitTimeOnExtreme / exerInfo.SpeedModifier)
                        {
                            totalDeltaTime = 0;
                            currentTransitionState = eTransitionState.OnExtremeToRest;
                        }
                        break;
                    case eTransitionState.OnExtremeToRest:
                        totalDeltaTime += Time.deltaTime;
                        exerInfo.TransitionAlpha = (totalDeltaTime / exerInfo.TransitionTimeExtremeToRest * exerInfo.SpeedModifier);
                        if (MirrorAnim)
                            CurrentPose = RigAnim.GetFinalPoseAtAnimPercentage(1 - exerInfo.TransitionAlpha).GetMirroredPose();
                        else
                            CurrentPose = RigAnim.GetFinalPoseAtAnimPercentage(1 - exerInfo.TransitionAlpha);
                        break;
                    default:
                        break;
                }

            }


        }

        //Ye Old Mayan code
        public void InitializeExercise(Exercise e, BehaviourParams param)
        {
            //throw new NotImplementedException();

            Editor_ExerciseName = "EstocadaFrontal";
            switch (e.Limb)
            {
                case Limb.Left:
                    MirrorAnim = true;
                    AlternateMirroring = false;
                    break;
                case Limb.Right:
                    MirrorAnim = false;
                    AlternateMirroring = false;
                    break;
                case Limb.Interleaved:
                    MirrorAnim = false;
                    AlternateMirroring = true;
                    break;
                case Limb.None:
                    MirrorAnim = false;
                    AlternateMirroring = false;
                    break;
                default:
                    break;
            }
            exerInfo.TransitionThreshold = param.Angle / 360;
            exerInfo.TransitionTimeRestToExtreme = param.ForwardSpeed;
            exerInfo.TransitionTimeExtremeToRest = param.BackwardSpeed;
            exerInfo.WaitTimeOnExtreme = param.SecondsInPose;
            exerInfo.WaitTimeOnRest = param.SecondsBetweenRepetitions;

            LoadExercise(Editor_ExerciseName);
            IsAnimExercisePreSetup = true;
            IsAnimPlaying = true;
            if (OnInitializeExerciseStart != null)
                OnInitializeExerciseStart.Invoke(this, new PrepareEventArgs(PrepareStatus.Preparing, Caller.Preview));


        }

        public void InitializeWebExercise(string s)
        {
            throw new NotImplementedException();
        }

        public void SetRestPose()
        {
            PlayRestPose();
            //throw new NotImplementedException();
        }

        public void StartExercise(bool isInInstruction)
        {
            //throw new NotImplementedException();
            if (IsExerciseInitialized)
                PlayAnimation();
            else
                Debug.Log("Exercise not initialized");
        }

        public void ResumePauseExercise()
        {
            if (IsAnimPlaying)
                PauseAnimation();
            else
                ResumeAnimation();
            //throw new NotImplementedException();
        }

        public void StartWebExercise(string jsonString)
        {
            throw new NotImplementedException();
        }

        public void StartExerciseNoParams()
        {
            //throw new NotImplementedException();
            InitializeExercise(new Exercise(Movement.EstocadaFrontalLarga, Limb.Interleaved), new BehaviourParams(360, 1.1f, 0.9f, 2, 3));
        }

        public void StopExercise()
        {
            StopAnimation();
            //throw new NotImplementedException();
        }

        public event EventHandler<RepetitionStartEventArgs> OnRepetitionStart;

        public event EventHandler OnRepetitionEnd;

        public event EventHandler OnSubrepetitionEnd;

        public event EventHandler OnRepetitionReallyStart;

        public event EventHandler<PrepareEventArgs> OnInitializeExerciseStart;

        public event EventHandler<PrepareEventArgs> OnInitializeExerciseEnd;
        //-- 


        //public event EventArgs OnRest;
    }

    public enum eTransitionState
    {
        None,
        OnWarmUp,
        OnRest,
        OnRestToExtreme,
        OnExtreme,
        OnExtremeToRest
    }

    public enum eExerciseTypes
    {
        None = 0,
        AbducciónDeCaderaEnDecúbitoLateral = 10000,
        AducciónResistidaEnPlanoEscapular = 20000,
        DesplazamientoLateralConPaso_100 = 30000,
        DesplazamientoLateralConPaso_75 = 30001,
        DesplazamientoLateralConPaso_50 = 30002,
        DesplazamientoLateralConPaso_25 = 30003,
        DesplazamientoLateralConSalto_100 = 40000,
        DesplazamientoLateralConSalto_75 = 40001,
        DesplazamientoLateralConSalto_50 = 40002,
        DesplazamientoLateralConSalto_25 = 40003,
        ElevaciónDeHombroEnPlanoEscapularConBastón = 50000,
        ElevaciónEnPuntaDePies_Nada = 60000,
        ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral = 70000,
        ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral = 80000,
        ElongaciónBandaIliotibial = 90000,
        ElongaciónCuádriceps = 100000,
        ElongaciónIsquiotibiales_TrícepsSural = 110000,
        EquilibrioBípedo = 120000,
        EquilibrioMonopodal = 130000,
        EquilibrioSedenteEnBalónSuizo = 140000,
        EstocadaFrontalCorta = 150000,
        EstocadaFrontalCortaConTorsiónDeTronco_90 = 160000,
        EstocadaFrontalCortaConTorsiónDeTronco_60 = 160001,
        EstocadaFrontalCortaConTorsiónDeTronco_45 = 160002,
        EstocadaFrontalLarga = 170000,
        EstocadaFrontalLargaConTorsiónDeTronco_90 = 180000,
        EstocadaFrontalLargaConTorsiónDeTronco_60 = 180001,
        EstocadaFrontalLargaConTorsiónDeTronco_45 = 180002,
        EstocadaLateral = 190000,
        ExtensiónDeCaderaEnProno = 200000,
        ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_90 = 210000,
        ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_60 = 210001,
        ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_45 = 210002,
        ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalBípedo45ºF_45_BIPEDO = 220000,
        ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_45 = 230000,
        ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_90 = 230001,
        ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_90 = 240000,
        ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_60 = 240001,
        ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_45 = 240002,
        ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_45 = 250000,
        ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_90 = 250001,
        ExtensiónDeRodillaConRodillo_Unilateral = 260000,
        ExtensiónDeRodillaEnSedente_Unilateral = 270000,
        ExtensiónDeRodillasConRodillo_Bilateral = 280000,
        ExtensiónDeRodillasEnSedente_Bilateral = 290000,
        ExtensiónHorizontalDeHombrosEnSupino = 300000,
        FlexiónDeCaderaEnSupino = 310000,
        FlexiónDeCodoEnBípedo_Unilateral_90 = 320000,
        FlexiónDeCodoEnBípedo_Unilateral_0 = 320001, // Ya no se usa
        FlexiónDeCodosEnBípedo_Bilateral_90 = 330000,
        FlexiónDeCodosEnBípedo_Bilateral_0 = 330001, // Ya no se usa
        FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_CIMA = 340000,
        FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_FRENTE = 340001,
        FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_NUCA = 340002,
        FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_90 = 350000,
        FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_45 = 350001,
        FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_90 = 360000,
        FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_45 = 360001,
        FlexiónDeHombrosEnBípedoConBastón_Bilateral = 370000,
        FlexiónDeRodillaEnProno_Unilateral = 380000,
        FlexiónDeRodillaEnSupino_Unilateral = 390000,
        FlexiónDeRodillasEnProno_Bilateral = 400000,
        FlexiónDeRodillasEnSupino_Bilateral = 410000,
        MantenerPosiciónExtrema_EtapaAvanzada_Ninja = 420000,
        MantenerPosiciónExtrema_EtapaAvanzada_Encestar = 420001,
        MantenerPosiciónExtrema_EtapaAvanzada_MusloArribaBrazosAdelanteYAtrás = 420002,
        MantenerPosiciónExtrema_EtapaAvanzada_PosturaDelÁrbol = 420003,
        MantenerPosiciónExtrema_EtapaAvanzada_Reloj = 420004,
        MantenerPosiciónExtrema_EtapaAvanzada_BrazosDiagonal = 420005,
        MantenerPosiciónExtrema_EtapaInicial_CuerdaFloja = 430000,
        MantenerPosiciónExtrema_EtapaInicial_CuerdaFlojaBrazoDerecho = 430001,
        MantenerPosiciónExtrema_EtapaInicial_CuerdaFlojaBrazoIzquierdo = 430002,
        MantenerPosiciónExtrema_EtapaInicial_FlexiónDeRodillasBrazosHorizontal = 430003,
        MantenerPosiciónExtrema_EtapaInicial_FlexiónDeRodillasBrazoDelanteYAtrás = 430004,
        MantenerPosiciónExtrema_EtapaIntermedia_RodillaYBrazosArriba = 440000,
        MantenerPosiciónExtrema_EtapaIntermedia_DominarBalón = 440001,
        MantenerPosiciónExtrema_EtapaIntermedia_MusloYBrazosHorizontal = 440002,
        MantenerPosiciónExtrema_EtapaIntermedia_MusloHorizontalBrazosAlFrente = 440003,
        PénduloEnBípedoCon45ºDeFlexiónDeTronco = 450000,
        PénduloEnBípedoConFlexiónDe90ºDeTronco = 460001,
        PénduloEnProno = 470002,
        PrensaDePiernas_120 = 480000,
        PrensaDePiernas_90 = 480001,
        PrensaDePiernas_45 = 480002,
        RetracciónEscapularEnBípedoCon45ºFlexiónDeTronco_45_BIPEDO = 490000,
        RotaciónExternaDeHombroEnBípedo = 500000,
        RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral = 510000,
        RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral = 520000,
        RotaciónExternaDeHombrosEnSupino_Bilateral = 530000,
        RotaciónInternaDeHombroEnBípedo = 540000,
        RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral = 550000,
        RotaciónInternaDeHombroEnSupinoConBastónT_Unilateral = 560000,
        RotaciónInternaDeHombrosEnSupino_Bilateral = 570000,
        SaltoBipodal_EXT_ROD = 580000,
        SaltoMonopodal = 590000,
        Sentadilla = 600000,
        SentadillaAsistidaConCamilla = 610000,
        SentadillaConBalónSuizo = 620000,
        SentadillaMonopodal = 630000,
        SubirEscalon_Frontal_SubeDerechaBajaDerecha = 640000,
        SubirEscalon_Frontal_SubeDerechaBajaIzquierda = 640001,
        SubirEscalon_Frontal_SubeIzquierdaBajaIzquierda = 640002,
        SubirEscalon_Frontal_SubeIzquierdaBajaDerecha = 640003,
        PlantiflexiónDeTobilloSedenteEnCamilla = 650000,
        ExtensiónDiagonalDeHombrosEnSupino_DerechaArriba = 660000,
        ExtensiónDiagonalDeHombrosEnSupino_IzquierdaArriba = 670000,
        FlexiónHorizontalResistidaDeHombros_BípedoBilateral = 680000,
        RotaciónDeHombrosAsistidaConBastón_DecúbitoSupino = 690000,
        RecogiendoYGuardandoConAmbasManos_BrazosArribaIzquierda = 700000,
        RecogiendoYGuardandoConAmbasManos_BrazosArribaDerecha = 700001,
        RecogiendoYGuardandoConAmbasManos_BrazosAbajoIzquierda = 700002,
        RecogiendoYGuardandoConAmbasManos_BrazosAbajoDerecha = 700003,
        SubirEscalón_Lateral = 710000,
        FlexiónDeRodillaAutoAsistidaEnSupino_Unilateral = 720000,
        EquilibrioBipedoConMovimientoDeMMSS_ArribaIzquierda = 730000,
        EquilibrioBipedoConMovimientoDeMMSS_ArribaDerecha = 730001,
        EquilibrioBipedoConMovimientoDeMMSS_AbajoIzquierda = 730002,
        EquilibrioBipedoConMovimientoDeMMSS_AbajoDerecha = 730003,
        EquilibrioMonopodalConMovimientoDeMMSS_ArribaIzquierda = 740000,
        EquilibrioMonopodalConMovimientoDeMMSS_ArribaDerecha = 740001,
        EquilibrioMonopodalConMovimientoDeMMSS_AbajoIzquierda = 740002,
        EquilibrioMonopodalConMovimientoDeMMSS_AbajoDerecha = 740003,
        EquilibrioMonopodalConMovimientoDeMiembroContralateral_PiernaAlFrente = 750000,
        EquilibrioMonopodalConMovimientoDeMiembroContralateral_PiernaAlLado = 750001,
        EquilibrioMonopodalConMovimientoDeMiembroContralateral_PiernaAtrás = 750002,
        EquilibrioSedenteEnBalónSuizoConPiernaDeApoyoExtendida = 760000,
        ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalProno_90_PRONO = 770000,
        ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalBípedo90ºF_90_BIPEDO = 780000,
        RetracciónEscapularEnBípedoCon90ºFlexiónDeTronco_90_BIPEDO = 790000,
        RetracciónEscapularEnProno_90_PRONO = 800000,
        SaltoBipodalLlevandoRodillasAlPecho_FLEX_ROD = 810000,
        EquilibrioSedenteEnBalónSuizoConPlatilloDeFreeman = 820000,
        ElevaciónEnPuntaDePiesEnStep_Step = 830000,
        RecogiendoYGuardandoConUnaMano_BrazoArribaIzquierda = 840000,
        RecogiendoYGuardandoConUnaMano_BrazoArribaDerecha = 840001,
        RecogiendoYGuardandoConUnaMano_BrazoAbajoIzquierda = 840002,
        RecogiendoYGuardandoConUnaMano_BrazoAbajoDerecha = 840003,

    }

    public class ExerciseInfo2
    {
        public eExerciseTypes ExerciseType;

        public ExerciseInfo2()
        {
            ExerciseType = eExerciseTypes.None;
        }
    }
}
