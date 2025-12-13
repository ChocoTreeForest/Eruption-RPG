# Eruption RPG
<img width="706" height="1254" alt="title" src="https://github.com/user-attachments/assets/8d1d9808-4b86-44a5-a37a-bdabcfe8644e" />

## 프로젝트 소개
Eruption RPG는 로그라이트 RPG 게임입니다.

화산 폭발하듯 한 번에 수십, 수백 레벨 업을 할 수 있는 시원시원한 플레이가 가능한 것이 특징입니다.

게임을 반복 플레이하여 강력한 장비를 획득하고, 나만의 장비 조합을 갖춘 뒤, 더 높은 레벨을 목표로 하세요!

게임 플레이 소개 영상: https://youtu.be/-wfqo07hlB4?si=brKOF17O3tJTemCR

![gameplay](https://github.com/user-attachments/assets/28fd8cae-be23-4d72-981f-4f05d586bf00)
![gameplay2](https://github.com/user-attachments/assets/77c0cfaf-b8b2-4946-9b08-45013b90acc1)

## 게임 플레이 정보
- **기본 조작법**

  - 화면을 홀드하면 캐릭터는 그 방향으로 움직입니다.
 
    ![move](https://github.com/user-attachments/assets/d8021802-a2e7-471b-bca4-5dda275a79ca)

- **주요 시스템**

  - 랜덤 인카운터: 플레이어가 움직이면 인카운터 게이지가 상승하고, 인카운터 게이지의 밸류가 랜덤 값 이상이 되면 몬스터가 인카운터됩니다.
    
    몬스터는 해당 레벨 존에 등장하는 몬스터들 중 랜덤으로 등장합니다. 랜덤 값은 게임 시작 시, 몬스터 인카운터 시 재설정됩니다.

    ![randomencounter](https://github.com/user-attachments/assets/53e535e2-41f1-4de5-a1d4-b83678a506ed)

  - 심볼 인카운터: 보스 몬스터는 심볼 인카운터 방식으로 인카운터됩니다.
 
    필드에 배치되어 있는 보스 몬스터에 접촉하면 인카운터되며 전투가 시작됩니다.

    ![symbolencounter](https://github.com/user-attachments/assets/5059a549-4584-4412-a6f6-f6671526aa9f)

  - 전투 횟수(BP): 일반 몬스터와의 전투에서 승리 시 BP가 1 감소합니다.
 
    보스 몬스터와 전투 시 BP를 획득할 수 있습니다. 전투에서 패배 시 BP가 3 감소합니다.

    BP를 전부 소모하면 게임 오버됩니다.

    ![battle](https://github.com/user-attachments/assets/f2d114c8-aa77-438e-b19d-5e540b3b085b)
    ![gameover](https://github.com/user-attachments/assets/84753597-a0a0-435c-b203-b97731e0ead5)

  - 프리셋: 스테이터스 프리셋과 장비 프리셋 기능이 있습니다.
 
    스테이터스 프리셋을 통해 스테이터스 자동 분배 기능을 활성화하고, 분배 비율을 설정하여,
    전투가 끝나면 설정한 분배 비율에 따라 스테이터스가 자동 분배됩니다.

    ![statuspreset](https://github.com/user-attachments/assets/b5697b9b-6016-4b22-9bd4-1af21076d6c0)

    장비 프리셋을 통해 장비를 빠르게 변경할 수 있습니다.

    ![equippreset](https://github.com/user-attachments/assets/a3068f78-8ec6-4b5c-99b6-bae71563cade)

  - 무한 모드: 전투에서 패배할 때까지 계속해서 전투할 수 있는 모드입니다.
 
    몬스터는 점점 강해지며 무한 모드에서만 획득할 수 있는 장비도 존재합니다.

    ![infinitymode](https://github.com/user-attachments/assets/5816c422-90c3-40be-a6e1-02b3b7daa295)
    
## 사용 기술 및 개발 환경
- Unity 2022.03 버전
- C#
- Photoshop을 사용해 캐릭터와 일부 몬스터 제작
- 싱글톤 패턴, Data Transfer Object (DTO)
- Scriptable Object로 몬스터 및 아이템 데이터 관리
- PlayerPrefs를 사용해 게임 내 설정 데이터 관리
- JSON 파일로 전투 로그 구현
- JSON 파일로 게임 데이터 저장
- Localization 패키지를 사용해 현지화
- Admob을 사용해 인앱 광고 구현
## 게임 설치 방법
구글 플레이 스토어에 'Eruption RPG' 검색 또는 https://play.google.com/store/apps/details?id=com.SeungJin.EruptionRPG 해당 링크에서 다운로드
## 사용 소스 및 참고 자료
- Pixel Art Top Down - Basic 애셋
- GROUND TILESETS RULE TILES 애셋
- Localization 패키지
- Admob
- 아이콘: https://game-icons.net/
- 몬스터: Rド http://rpgdot3319.g1.xrea.com/
- 이펙트: Pipoya  https://pipoya.net/
- 魔王魂 https://maou.audio/
- OtoLogic https://otologic.jp/
- 폰트: PF스타더스트 https://blog.naver.com/campanula913/221366697603
- 이 게임은 Inflation RPG를 모티브로 제작했습니다.
