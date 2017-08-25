﻿#if UNITY_2017_1_OR_NEWER

using System;
using UnityEngine.Playables;

namespace UTJ.Alembic
{
    public class AlembicShotPlayable : PlayableBehaviour
    {
        AlembicStreamPlayer m_StreamPlayer;
        public AlembicStreamPlayer streamPlayer
        {
            get
            {
                return m_StreamPlayer;
            }
            set
            {
                m_StreamPlayer = value;
                if( m_StreamPlayer != null )
                    m_StreamPlayer.m_ManualUpdateMode = true;
            }
        }

        bool m_RestoreState = false;
        bool m_OrgState;
        public float m_StartTimeOffset;
        public float m_EndTimeClipOff;
        public float m_TimeScale;
        public bool m_AutoActivateTarget;
        public AlembicPlaybackSettings.CycleType m_Cycle = AlembicPlaybackSettings.CycleType.Hold;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            if (streamPlayer == null)
                return;
            streamPlayer.m_PlaybackSettings.m_startTime = 0f;
            streamPlayer.m_PlaybackSettings.m_cycle = m_Cycle;
            streamPlayer.m_PlaybackSettings.m_timeOffset = (float)m_StartTimeOffset;
            streamPlayer.m_PlaybackSettings.m_endTime = (float)streamPlayer.m_PlaybackSettings.m_duration - m_EndTimeClipOff;
            streamPlayer.m_PlaybackSettings.m_timeScale = (float)m_TimeScale;
            streamPlayer.m_PlaybackSettings.m_Time = (float)playable.GetTime();
			streamPlayer.m_PlaybackSettings.m_OverrideTime = true;
            streamPlayer.m_PlaybackSettings.m_preserveStartTime = true;

            streamPlayer.ManualUpdate();
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);

            if (streamPlayer == null)
                return;

            if (m_AutoActivateTarget)
            {
                m_RestoreState = true;
                m_OrgState = streamPlayer.gameObject.activeInHierarchy;
                streamPlayer.gameObject.SetActive(true);
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);

            if (streamPlayer == null)
                return;

            if( m_AutoActivateTarget && m_RestoreState )
                streamPlayer.gameObject.SetActive(m_OrgState);

        }
    }
}

#endif
