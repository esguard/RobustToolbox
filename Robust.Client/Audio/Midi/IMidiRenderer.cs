using System;
using Robust.Client.Graphics;
using Robust.Shared.Audio.Midi;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;

namespace Robust.Client.Audio.Midi;

public enum MidiRendererStatus : byte
{
    None,
    Input,
    File,
}

public interface IMidiRenderer : IDisposable
{

    /// <summary>
    ///     The buffered audio source of this renderer.
    /// </summary>
    internal IClydeBufferedAudioSource Source { get; }

    /// <summary>
    ///     Whether this renderer has been disposed or not.
    /// </summary>
    bool Disposed { get; }

    /// <summary>
    ///     This controls whether the midi file being played will loop or not.
    /// </summary>
    bool LoopMidi { get; set; }

    /// <summary>
    ///     This increases all note on velocities to 127.
    /// </summary>
    bool VolumeBoost { get; set; }

    /// <summary>
    ///     The midi program (instrument) the renderer is using.
    /// </summary>
    byte MidiProgram { get; set; }

    /// <summary>
    ///     The instrument bank the renderer is using.
    /// </summary>
    byte MidiBank { get; set; }

    /// <summary>
    ///     The soundfont currently selected by the renderer.
    /// </summary>
    uint MidiSoundfont { get; set; }

    /// <summary>
    ///     The current status of the renderer.
    ///     "None" if the renderer isn't playing from input or a midi file.
    ///     "Input" if the renderer is playing from midi input.
    ///     "File" if the renderer is playing from a midi file.
    /// </summary>
    MidiRendererStatus Status { get; }

    /// <summary>
    ///     Whether the sound will play in stereo or mono.
    /// </summary>
    bool Mono { get; set; }

    /// <summary>
    ///     Whether to drop messages on the percussion channel.
    /// </summary>
    bool DisablePercussionChannel { get; set; }

    /// <summary>
    /// Whether to drop messages for program change events.
    /// </summary>
    bool DisableProgramChangeEvent { get; set; }

    /// <summary>
    ///     Gets the total number of ticks possible for the MIDI player.
    /// </summary>
    int PlayerTotalTick { get; }

    /// <summary>
    ///     Gets or sets (seeks) the current tick of the MIDI player.
    /// </summary>
    int PlayerTick { get; set; }

    /// <summary>
    ///     Gets the current tick of the sequencer.
    /// </summary>
    uint SequencerTick { get; }

    /// <summary>
    ///     Gets the Time Scale of the sequencer in ticks per second. Default is 1000 for 1 tick per millisecond.
    /// </summary>
    double SequencerTimeScale { get; }

    /// <summary>
    ///     Start listening for midi input.
    /// </summary>
    bool OpenInput();

    /// <summary>
    ///     Start playing a midi file.
    /// </summary>
    /// <param name="buffer">Bytes of the midi file</param>
    bool OpenMidi(ReadOnlySpan<byte> buffer);

    /// <summary>
    ///     Stops listening for midi input.
    /// </summary>
    bool CloseInput();

    /// <summary>
    ///     Stops playing midi files.
    /// </summary>
    bool CloseMidi();

    /// <summary>
    ///     Stops all notes being played currently.
    /// </summary>
    void StopAllNotes();

    /// <summary>
    ///     Render and play MIDI to the audio source.
    /// </summary>
    internal void Render();

    /// <summary>
    ///     Loads a new soundfont into the renderer.
    /// </summary>
    void LoadSoundfont(string filename, bool resetPresets = false);

    /// <summary>
    ///     Invoked whenever a new midi event is registered.
    /// </summary>
    event Action<RobustMidiEvent> OnMidiEvent;

    /// <summary>
    ///     Invoked when the midi player finishes playing a song.
    /// </summary>
    event Action OnMidiPlayerFinished;

    /// <summary>
    ///     The entity whose position will be used for positional audio.
    ///     This is only used if <see cref="Mono"/> is set to True.
    /// </summary>
    EntityUid? TrackingEntity { get; set; }

    /// <summary>
    ///     The position that will be used for positional audio.
    ///     This is only used if <see cref="Mono"/> is set to True
    ///     and <see cref="TrackingEntity"/> is null.
    /// </summary>
    EntityCoordinates? TrackingCoordinates { get; set; }

    /// <summary>
    ///     Send a midi event for the renderer to play.
    /// </summary>
    /// <param name="midiEvent">The midi event to be played</param>
    void SendMidiEvent(RobustMidiEvent midiEvent);

    /// <summary>
    ///     Schedule a MIDI event to be played at a later time.
    /// </summary>
    /// <param name="midiEvent">the midi event in question</param>
    /// <param name="time"></param>
    /// <param name="absolute"></param>
    void ScheduleMidiEvent(RobustMidiEvent midiEvent, uint time, bool absolute);

    /// <summary>
    ///     Actually disposes of this renderer. Do NOT use outside the MIDI thread.
    /// </summary>
    internal void InternalDispose();
}
