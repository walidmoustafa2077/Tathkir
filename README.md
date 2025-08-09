# Tathkīr

**Tathkīr** is a desktop and API-based application designed to help Muslims stay connected to their daily spiritual practices.  
It provides accurate **Muslim prayer time reminders**, **athkar notifications**, **tasbeeh tracking**, and **Qur’an & Athkar reading schedules** — enriched with a library of **100+ reciters** for Qur’an audio playback.  

Whether at home or traveling, Tathkīr supports **multiple locations** for prayer times, making it ideal for those who move between cities or countries.  
Users can listen to daily athkar, read from the Qur’an directly in the app, or play recitations from their favorite qari (reciter).  

---

## Key Features

- Precise Muslim prayer times for multiple locations  
- Thikr (dhikr) reminders  
- Tasbeeh counter for dhikr tracking  
- Athkar reading (morning, evening, after prayer)  
- Qur’an reading & listening with 100+ reciters  
- Multi-location support with accurate calculation methods  
- Desktop widget showing prayer times, countdown to next prayer, and current date  
- Tray widget for quick access to upcoming prayer times  
- Modern **WPF** desktop interface with right-to-left (RTL) Arabic support  
- **.NET Core API** backend with modular service design  
- Docker-ready API for easy deployment  

---

## Tech Stack

**Frontend (WPF)**  
- .NET 8 WPF  
- MVVM architecture  
- Material Design in XAML  

**Backend (API)**  
- ASP.NET Core Web API  
- Entity Framework Core  
- Modular service interfaces  
- Docker-ready configuration  

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)  
- [Docker](https://www.docker.com/) (for API deployment)  

### Running the API Locally
```bash
cd Tathkir.API
dotnet run
