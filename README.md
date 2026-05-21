# README

## ToyWar: Invader in Dreams
> **"아이들의 숙면을 방해하는! 가장 순수한 방어전이 시작됩니다."**
>
> **ToyWar**는 아이의 머릿속 상상력을 바경으로 한 **VR 타워 디펜스 액션 게임**입니다. 현실의 악몽을 투영한 유령과 로봇들에 맞서, 고양이 인형과 장난감 로봇들을 지휘하세요!

<table align="center">
  <tr>
    <td align="center" width="49%">
      <img width="400" height="207" alt="Image" src="https://github.com/user-attachments/assets/5ac5ed68-9e74-4bc8-8484-033634cda2b1" />
    </td>
    <td align="center" width="49%">
      <img width="400" height="207" alt="Image" src="https://github.com/user-attachments/assets/71ce5746-a283-40b4-a2a2-c23afe9fe1de" />
    </td>
  </tr>
</table>

---

## 프로젝트 개요 | Project Overview
- **Platform:** Meta Quest 2 / PC VR
- **Engine:** Unity 6
- **Input:** OpenXR / Unity Input System (Action-based)
- **Graphics:** URP
- **Language:** C#
- **Core Concept:** 전략적 인형 배치 + 실시간 1인칭 슈팅 기반의 하이브리드 VR 디펜스
> 아케이드 게임 수록곡 ToyWar에서 영감을 받은 오마주 타이틀입니다. 

---

## 주요 특징 | Core Features

### 1. 시네마틱한 내러티브 컨셉
어두운 밤, 침대 밑과 옷장, 어두운 복도 너머와 창문 밖에서 찾아오는 침략자들로부터 나를 지키는 '장난감 수호대'라는 감성적인 스토리를 VR의 몰입감으로 극대화했습니다. 

### 2. 전략적 3단계 진환 시스템
각 유닛은 **3단계의 업그레이드 경로**를 가지며, 총 18종의 개성 있는 아군 변형을 제공합니다.

<table align="center">
  <tr font-weight="bold">
    <td align="center">아군 유닛</td>
    <td align="center">1단계 (Basic)</td>
    <td align="center">2단계 (Advanced)</td>
    <td align="center">3단계 (Ultimate)</td>
  </tr>
  <tr>
    <td align="center"><b>고양이 인형</b></td>
    <td align="center">권총</td>
    <td align="center">소총</td>
    <td align="center">기관총</td>
  </tr>
  <tr>
    <td align="center"><b>다람쥐 인형</b></td>
    <td align="center">수류탄</td>
    <td align="center">강화 수류탄</td>
    <td align="center">RPG-7</td>
  </tr>
  <tr>
    <td align="center"><b>장난감 상자</b></td>
    <td align="center">소형 로봇</td>
    <td align="center">고속 로봇</td>
    <td align="center">비행 로봇</td>
  </tr>
</table>

### 3. VR 최적화 UX/UI
- **Controller-Centric UI:** 플레이어의 시선을 방해하지 않도록 체력과 잔탄 정보를 컨트롤러 위치에 고정하여 몰입감을 유지했습니다.
- **Ray Interactor:** 직관적인 레이 인터랙션을 통해 원거리에서도 정교하게 인형을 배치하고 강화할 수 있습니다.

---

## 핵심 기능 | Key Features

### 1. 고급 오브젝트 풀링 시스템 (Advanced Object Pooling)
VR 환경에서 프레임 드랍을 방지하기 위해 가장 공을 들인 부분입니다.
- **고안 배경:** 장난감 총을 발사하는 느낌을 살리기 위해 레이캐스트 대신 탄환을 물리 오브젝트로 생성해 사용하는 방법을 채택하였으나, 동시에 생성되는 오브젝트가 프레임 드랍을 유발했습니다.
- **핵심 로직:** 캐릭터가 스폰되면, 새로운 오브젝트 풀을 생성하거나 이미 존재하는 총알을 풀의 자식으로 재활용합니다.
- **최적화 타겟:** 총알, 이펙트, 다량의 잡몹(미라, 유령) 등 잦은 생성/소멸이 발생하는 객체.
- **성과:** 단순 활성화/비활성화 연산만으로 수천발의 탄환을 제어하며, **안정적인 72~90 FPS를 유지**했습니다.

### 2. 유연한 상속 및 인터페이스 구조 (Unified Inheritance)
확장성을 고려하여 아군과 적군의 공통 로직을 추상화했습니다.
- **Base Unit Class:** 공격력, 체력, 상태 이상 등 공통 프로퍼티를 관리합니다.
- **Modular Design:** 새로운 보스나 새로운 장난감이 추가되더라도 코드 수정 없이 데이터 세팅만으로 추가가 가능한 **데이터 주도형 설계**를 지향했습니다.

### 3. VR Input Action 기반 전투 시스템
- `Unity Input System`을 활용하여 VR 환경에 최적화된 입력을 구현하였습니다. 
- 텔레포트 이동 방식을 배제하고 고정 위치 디펜스 방식을 채택하여 VR 멀미를 원천적으로 차단했습니다.

---

## 기술적 고민 | Troubleshooting

### [고민했던 주제 1: 대규모 물량 공세 시의 성능 저하]
- **현상:** 게임 후반부로 갈수록 악몽 군단(미라, 유령 등)이 화면에 수십 마리 이상 동시에 스폰되면서 물리 연산과 데이터 생성 비용이 중첩되어 VR 기기 내부 프레임이 급격히 떨어지는 현상이 발생했습니다.
- **해결:** 스테이지별로 유저가 체감해야 하는 '전체 생성 총 몬스터 수'를 순수 정수 형태의 변수 데이터로만 관리하도록 설계했습니다. 또한, 동시 최대 생성 수의 임계값을 두어 이를 초과하면 데이터 상으로만 대기열에 머물도록 제안했습니다.
- **성과:** 유저 입장에서는 끊임없이 몬스터가 밀려오는 듯한 풍성한 물량 공세 연출을 그대로 유지하면서도, 하드웨어 레벨에서는 실제 런타임 객체 수를 완벽히 통제하여 프레임 드랍을 해결했습니다. 

### [고민했던 주제 2: 다수의 터렛 배치 시 적 감지 로직의 CPU 병목 현상]
- **현상:** 유저가 고양이 인형, 다람쥐 인형 등 수십 개의 타워를 필드에 배치했을 때, 타워의 개수에 비례하여 CPU 사용량이 기하급수적으로 증가하는 프레임 드랍이 발생했습니다.
- **해결:** 매 업데이트마다 탐색하는것이 아닌, 일정 주기마다 타겟을 업데이트하도록 제한하였습니다.
- **성과:** 연산 비용을 아끼는것뿐 만 아닌, 아군 인형이 적을 감지하는 과정이 더욱 자연스러워지는 연출을 만들어낼 수 있었습니다. 

---

## 스크린샷 | Screenshots
<table align="center">
  <tr>
    <td align="center" width="49%">
      <img width="1295" height="692" alt="Image" src="https://github.com/user-attachments/assets/42a06968-b41a-4d7a-a1ef-d10ca9486cb3" />
      <br><b>장난감 로봇(탱커)</b>
    </td>
    <td align="center" width="49%">
      <img width="1681" height="704" alt="Image" src="https://github.com/user-attachments/assets/ed67a309-0750-42d9-981a-c156c134198d" />
      <br><b>고양이 인형(기관총)과 다람쥐 인형(폭탄) </b>
    </td>
  </tr>
  <tr>
    <td align="center" width="49%">
      <img width="1803" height="799" alt="Image" src="https://github.com/user-attachments/assets/59910bf8-276b-446d-a323-9292783107c7" />
      <br><b>플레이어 강화</b>
    </td>
    <td align="center" width="49%">
      <img width="1718" height="794" alt="Image" src="https://github.com/user-attachments/assets/6a9ba198-3256-4543-bf09-61a6485af6c7" />
      <br><b>인형 강화</b>
    </td>
  </tr>
</table>

---

## 프로젝트 회고 | Lessons

### 1. 기획과 엔지니어링 사이의 균형잡기
- 장난감 총의 감성이라는 핵심 기획 요소를 위해 모든 탄환을 물리 투사체 객체로 고집했던 선택은, 초반에 심각한 프레임 드랍이라는 문제가 되었습니다. 하지만 기획을 포기하는 대신 오브젝트 풀링과 시분할 적 감지 로직을 통해 기획 달성과 성능 최적화를 모두 이루었습니다.

### 2. VR 플랫폼의 하드웨어 제약과 최적화의 필연성
- Meta Quest 2와 같은 VR 플랫폼은 PC와 연결되어 있어도 연산 자원이 한정되어 있기에 심한 프레임 드랍 문제가 발생한다는것을 배웠습니다. 대규모 물량 공세 상황에서 가상 몬스터 카운팅 시스템을 도입하고 CPU/GPU 병목을 해결해 나가면서, '코드가 하드웨어에 미치는 영향'을 실시간으로 체감할 수 있었습니다.

### 3. 확장성을 고려한 코드 아키텍처 설계의 중요성
- 게임에 등장하는 적들의 다양성을 고려하여, 공통 로직을 구현해 동일 컴포넌트를 공유하는 컴포넌트 중심 구조로 설계하였습니다. 기획은 언제나 변하고 콘텐츠는 확장된다는 전제하에, 데이터 주도형 구조를 설계하는 것이 프로젝트 중후반부의 개발 속도를 얼마나 폭발적으로 가속하는지 몸소 체험했습니다
