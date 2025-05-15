# OpenAI Whisper Audio to Text

## 📌 Features

- Simple CLI app that transcribes MP3 audio using OpenAI's Whisper API
- Supports `.env` for secure API key management
- Processes Turkish (and other languages) audio files
- Clean console output of the transcription result

---

## 🚀 Getting Started

1. 📄 Copy the `.env.example` file and rename it to `.env`.
2. 🛠️ Add your OpenAI API key:
    ```env
    OPENAI_API_KEY=sk-xxx...
    ```
3. ▶️ Run the app:
    ```bash
    dotnet run
    ```

> ❗ Make sure your `.env` file is set to **Copy if newer** in the file properties.
    
## 💬 Example Output
![Screenshot](/screenshots/OpenAIWhisperAudioToText/example1.png) 
_Source: [YouTube](https://www.youtube.com/watch?v=-Mbr_ocqNeU)_
