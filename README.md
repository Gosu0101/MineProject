# MineProject

  > 채굴, 판매, 업그레이드, 지역 확장을 하나의 루프로 묶은 Unity 기반 3D 마이닝 게임 프로젝트

  [![Unity](https://img.shields.io/badge/Unity-6000.1.14f1-black?logo=unity)](#)
  [![C#](https://img.shields.io/badge/C%23-Game%20Programming-239120?logo=c-sharp)](#)
  [![URP](https://img.shields.io/badge/Render%20Pipeline-URP-blue)](#)
  [![Platform](https://img.shields.io/badge/Platform-PC-lightgrey)](#)

  ## Project Summary

  MineProject는 플레이어가 광물을 채굴하고, 자원을 판매해 골드를 얻고, 그 골드로 장비와 능력을 성장시키며 더 깊은 채굴
  구역을 개척하는 **성장형 3D 채굴 게임**입니다.
  단순한 채굴 액션에 그치지 않고, **인벤토리 관리**, **판매 전략**, **장비 업그레이드**, **신규 지역 구매**까지 연결되는
  루프를 설계한 것이 핵심입니다.

  ## Demo

  - 시연 영상: `[영상 링크]`
  - 플레이 이미지: `[스크린샷]`

  ```text
  - 메인 화면
  - 채굴 장면
  - 상점 / 업그레이드 UI
  - 신규 지역 해금 장면
  ```
  ## Why This Project

  이 프로젝트는 단순히 블록을 부수는 데서 끝나는 게임이 아니라,

  - 채굴 가능한 자원을 구분하고
  - 인벤토리에 보관하고
  - 판매를 통해 재화를 축적하고
  - 재화를 다시 성장 요소에 투자하는

  하나의 경제 루프와 성장 루프를 구현하는 데 목적이 있었습니다.

  즉, 플레이어가 반복 행동을 통해 점점 더 효율적으로 플레이할 수 있도록 만드는 구조를 직접 설계하고 구현한 프로젝트입니
  다.

  ## Core Loop

  1. 광물을 채굴한다.
  2. 드롭된 자원을 획득한다.
  3. 자원을 판매해 골드를 획득한다.
  4. 곡괭이, 가방, 채굴 속도를 업그레이드한다.
  5. 새로운 채굴 지역을 구매한다.
  6. 더 깊고 더 좋은 자원을 다시 채굴한다.

  ## Key Features

  ### 1. 블록 채굴 시스템

  - 블록마다 경도, 체력, 판매 가치를 별도 데이터로 관리
  - 곡괭이 성능이 부족하면 채굴 효율이 떨어지도록 설계
  - 블록 파괴 시 자원 드롭과 다음 레이어 생성 로직이 연결됨

  ### 2. 자원 획득 및 인벤토리

  - 블록 파괴 후 아이템이 드롭되고 플레이어가 직접 습득
  - 인벤토리에 자원 종류별로 저장
  - 가방 업그레이드로 보관 가능한 슬롯 수 확장

  ### 3. 판매 시스템

  - 전체 판매
  - 특정 자원만 선택 판매
  - 판매 결과가 골드 UI에 즉시 반영되도록 구현

  ### 4. 업그레이드 시스템

  - 곡괭이 강화
  - 가방 용량 증가
  - 채굴 속도 증가
  - 업그레이드 결과가 UI와 장비 비주얼에 반영

  ### 5. 지역 확장 시스템

  - 특정 구역에서 골드를 사용해 신규 채굴 지역 구매
  - 새로운 스포너를 생성해 플레이 공간이 확장되는 구조

  ### 6. 게임 상태 및 인터랙션

  - 우클릭 기반 상호작용
  - UI 패널 열기/닫기
  - 게임 클리어 및 재시작 처리
  - 오디오 피드백 연결

  ## Tech Stack

  - Engine: Unity 6 (6000.1.14f1)
  - Language: C#
  - Rendering: Universal Render Pipeline (URP)
  - Input: Unity Input System
  - Data Management: ScriptableObject
  - UI: Unity UI, TextMeshPro
  - Version Control: GitHub

  ## Technical Highlights

  ### ScriptableObject 기반 데이터 분리

  블록과 곡괭이 데이터를 코드에서 하드코딩하지 않고 ScriptableObject로 분리했습니다.

  - BlockData
  - PickaxeData
  - PickaxeTierList
  - UpgradeData

  이 구조를 통해:

  - 밸런스 조정이 쉬워지고
  - 새로운 광물 / 곡괭이 추가가 간단해지고
  - 데이터와 로직의 결합도를 낮출 수 있었습니다.

  ### Singleton 기반 매니저 구조

  주요 전역 시스템은 Singleton 패턴으로 관리했습니다.

  - GameManager
  - PlayerManager
  - AudioManager
  - SpawnerScript

  이를 통해 씬 내 여러 시스템이 중앙 상태를 공유하고,
  UI 갱신이나 상호작용 로직을 단순하게 연결할 수 있도록 구성했습니다.

  ### 성장형 게임 루프 설계

  이 프로젝트에서 중요했던 점은 개별 기능 구현보다도
  각 기능이 서로 연결되어 반복 플레이 동기를 만드는 구조였습니다.

  - 채굴은 판매로 이어지고
  - 판매는 업그레이드로 이어지고
  - 업그레이드는 더 깊은 채굴과 더 빠른 성장으로 이어집니다.

  즉, 기능 단위가 아니라 플레이 경험 단위로 시스템을 엮는 것에 집중했습니다.

  ## Controls

  - W / A / S / D : 이동
  - Mouse : 시점 회전
  - Left Click : 채굴
  - Right Click : 상호작용
  - Space : 점프
  - B : 지역 구매
  - Q : 퀘스트 UI 토글
  - E : 인벤토리 UI 토글
  - R : 아이템 UI 토글
  - Esc : 일시정지
  - T : 현재 씬 재시작
  - H : 플레이어 위치 초기화

  ## Project Structure

  Assets
  ├─ Scenes
  │  └─ MainLjw.unity
  ├─ Scripts
  │  ├─ GameManager.cs
  │  ├─ PlayerManager.cs
  │  ├─ BlockController.cs
  │  ├─ SpawnerScript.cs
  │  ├─ ItemController.cs
  │  ├─ PurchaseZone.cs
  │  ├─ StoreUIController.cs
  │  ├─ ShopUI.cs
  │  ├─ UpgradeStationController.cs
  │  ├─ UpgradeUI.cs
  │  ├─ GoldUI.cs
  │  ├─ AudioManager.cs
  │  ├─ Inventory
  │  └─ Pickaxe
  ├─ Animations
  ├─ region
  Packages
  ProjectSettings

  ## What I Focused On

  - 반복 플레이가 자연스럽게 이어지는 게임 루프 설계
  - 데이터와 로직을 분리한 구조 설계
  - UI, 상점, 업그레이드, 자원 시스템 간의 연결
  - 플레이어가 성장 체감을 바로 느낄 수 있는 피드백 설계

  ## Troubleshooting / Notes

  현재 저장소 기준으로 확인한 사항:

  - 권장 Unity 버전은 6000.1.14f1
  - 실제 플레이 씬 후보는 Assets/Scenes/MainLjw.unity
  - Build Settings에는 존재하지 않는 SampleScene 경로가 잡혀 있어 실행 전에 씬 확인이 필요할 수 있음

  ## Retrospective

  이 프로젝트를 통해 단순 기능 구현보다 더 중요한 것은
  게임의 각 시스템이 어떻게 연결되어 플레이 동기를 만드는가라는 점을 배웠습니다.

  특히 아래와 같은 부분을 경험할 수 있었습니다.

  - ScriptableObject를 활용한 데이터 중심 설계
  - 채굴, 판매, 업그레이드, 확장을 잇는 성장 구조 설계
  - UI와 게임 로직을 연결하는 상태 관리 방식
  - 프로토타입 단계에서 빠르게 반복 구현하는 Unity 워크플로우

  ## Future Improvements

  - 저장 / 불러오기 기능
  - 자원 희귀도 및 등급 시스템 확장
  - 튜토리얼 및 퀘스트 흐름 보강
  - 업그레이드 밸런스 개선
  - 빌드 세팅 정리 및 배포 버전 최적화
  - 사운드 / 이펙트 / UX polish 강화

  ## Contributors

  - Gosu0101 (https://github.com/Gosu0101)
  - Grandheaven (https://github.com/Grandheaven)
  - alswo10 (https://github.com/alswo10)

  ## Repository

  - GitHub: https://github.com/Gosu0101/MineProject
