# 🎵 Pulse Climb

> Jogo educacional em 3D desenvolvido em Unity para a disciplina de **Desenvolvimento de Jogos e Realidade Virtual** — IFPI, Curso Superior Tecnólogo em Análise e Desenvolvimento de Sistemas.

***

## 👥 Equipe — Grupo 06

| Nome | Papel |
|------|-------|
| Roger | Desenvolvedor |
| Elinne | Desenvolvedora |
| Maria Fernanda | Desenvolvedora |
| Fabricyo | Desenvolvedor |

***

## 🎮 Sobre o Jogo

**Pulse Climb** é um jogo educacional do gênero *Rhythm* com perspectiva de *vertical scroller*. O jogador deve escalar plataformas no ritmo da música, aprendendo conceitos de padrões rítmicos e coordenação motora ao longo dos níveis.

### Elementos Sorteados (Awesome Game Idea Generator)

| Categoria | Elemento |
|-----------|----------|
| Gênero | Rhythm |
| Tema | One button, two functions |
| Interação | Observe |
| Forma | Vertical Scroller |
| Mecânica 1 | Pattern Building |
| Mecânica 2 | Jumping |
| Mecânica 3 | Exploration |

***

## 🎯 Objetivos Educacionais

- Desenvolver o senso rítmico e a percepção musical do jogador
- Estimular a coordenação entre estímulos sonoros e ações motoras
- Praticar reconhecimento de padrões através da mecânica de *beat*

***

## 🕹️ Como Jogar

1. Observe o padrão de batidas indicado pelo `BeatIndicator`
2. Salte nas plataformas **no ritmo certo** para ganhar impulso e subir mais alto
3. Evite cair abaixo da tela — a zona de morte encerra a fase
4. Explore os níveis disponíveis na tela de seleção de fase

**Controles:**
- `Espaço` / `Botão A (Gamepad)` — Pular / Função 2
- `ESC` — Pausar o jogo

***

## 🛠️ Tecnologias Utilizadas

- **Engine:** Unity (versão 2022.3 LTS ou superior)
- **Linguagem:** C#
- **Input System:** Unity New Input System
- **UI:** TextMesh Pro
- **Áudio:** Unity Audio Manager customizado
- **Dados:** ScriptableObjects (DifficultyConfig)

***

## 📁 Estrutura do Projeto

```
Assets/
├── Art/                  # Texturas, sprites e materiais visuais
├── Audio/                # Trilha sonora e efeitos sonoros
├── Data/                 # ScriptableObjects de configuração
├── Materials/            # Materiais Unity
├── Prefabs/              # Prefabs de plataformas, player e UI
├── Scenes/               # Cenas do jogo (Menu, Jogo, Seleção de Fase)
├── Scripts/              # Código-fonte C#
│   ├── AudioManager.cs
│   ├── BeatManager.cs
│   ├── BeatIndicator.cs
│   ├── CameraFollow.cs
│   ├── DeathZone.cs
│   ├── GameManager.cs
│   ├── HUDController.cs
│   ├── LevelSelectController.cs
│   ├── MainMenuController.cs
│   ├── OneWayPlatform.cs
│   ├── PauseManager.cs
│   ├── PlatformBehavior.cs
│   ├── PlatformSpawner.cs
│   └── PlayerController.cs
├── SkySeries Freebie/    # Skybox
├── TextMesh Pro/         # Fontes e UI
└── UI/                   # Assets de interface
```

***

## 🚀 Como Executar

### Online
> *[Pulse Climb - Play Unity](https://play.unity.com/en/games/c2a54b0e-7347-4f8f-9dad-344c6089597f/pulseclimb)*

### Local (a partir do código-fonte)
1. Clone o repositório:
   ```bash
   git clone https://github.com/mfeeee/pulse-climb.git
   ```
2. Abra o projeto no **Unity Hub** (Unity 2022.3+)
3. Abra a cena principal em `Assets/Scenes/`
4. Clique em **Play** no Editor

***

## 📚 Referências e Trabalho

- **Disciplina:** Desenvolvimento de Jogos e Realidade Virtual
- **Professor:** Denylson Melo
- **Instituição:** Instituto Federal de Educação, Ciência e Tecnologia do Piauí (IFPI)
- **Trabalho:** Trabalho 04 — Desenvolvimento de Protótipo de Jogo 3D
- **GDD:** Disponível via SUAP (Trabalho 02)