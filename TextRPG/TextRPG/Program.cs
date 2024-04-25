using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TextRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.MainMenu();
        }
    }

    public enum StringLength
    {
        ItemName = 20,
        ItemAbility = 10,
        ItemInfo = 60
    }

    class Menu
    {
        private int mSelect;
        private bool mIsNum;
        // 캐릭터 생성 (레벨, 이름, 직업, 공격력, 방어력, 체력, 골드)
        private Player mPlayer = new Player(1, "Chad", "전사", 10, 5, 100, 6000);
        private List<Item> mItems = new List<Item>();
        private List<Item> mPlayerItems = new List<Item>();

        // 메뉴가 생성 될 때 전체 아이템 생성
        public Menu()
        {
            // 아이템 생성 (아이템 이름, 공격력, 방어력, 아이템 정보, 가격)
            Item item1 = new Item("수련자 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000);
            mItems.Add(item1);
            Item item2 = new Item("무쇠 갑옷", 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000);
            mItems.Add(item2);
            Item item3 = new Item("스파르타의 갑옷", 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
            mItems.Add(item3);
            Item item4 = new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다.", 600);
            mItems.Add(item4);
            Item item5 = new Item("청동 도끼", 5, 0, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
            mItems.Add(item5);
            Item item6 = new Item("스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2500);
            mItems.Add(item6);
        }

        // 메인 메뉴
        public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                mIsNum = int.TryParse(Console.ReadLine(), out mSelect);
                if (mIsNum)
                {
                    switch (mSelect)
                    {
                        case 1:
                            // 상태 보기
                            StatusMenu();
                            continue;
                        case 2:
                            // 인벤토리
                            InventoryMenu();
                            continue;
                        case 3:
                            // 상점
                            StoreMenu();
                            continue;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                Console.ReadLine();
            }
        }

        // (플레이어 능력치 + 장비 능력치) 출력
        void PrintAbility(int playerAbility, int itemAbility)
        {
            if (itemAbility > 0)
            {
                Console.WriteLine($"{playerAbility + itemAbility} (+{itemAbility})");
            }
            else if (itemAbility < 0)
            {
                Console.WriteLine($"{playerAbility + itemAbility} ({itemAbility})");
            }
            else
            {
                Console.WriteLine($"{playerAbility}");
            }
        }

        // 상태 보기
        void StatusMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine();
                Console.WriteLine($"Lv. {mPlayer.mLevel:D2}");
                Console.WriteLine($"{mPlayer.mName} ({mPlayer.mClass})");
                Console.Write("공격력: ");
                PrintAbility(mPlayer.mPower, mPlayer.mItemPower);
                Console.Write("방어력: ");
                PrintAbility(mPlayer.mDefense, mPlayer.mItemDefense);
                Console.Write("체력: ");
                PrintAbility(mPlayer.mHp, mPlayer.mItemHp);
                Console.WriteLine($"Gold: {mPlayer.mGold} G");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                mIsNum = int.TryParse(Console.ReadLine(), out mSelect);
                if (mIsNum)
                {
                    switch (mSelect)
                    {
                        case 0:
                            return;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");

                }
                Console.ReadLine();
            }
        }

        // 문자열 길이 계산 (한글은 2칸, 공백은 1칸) // 문제점: 도중에 영어나 숫자가 섞여있으면 사용x
        int StringCount(string str)
        {
            string[] korean = str.Split(' ');
            int count = 0;
            foreach (string s in korean)
            {
                count += s.Length * 2;
            }
            count += korean.Length - 1;
            return count;
        }

        // 여백 만들기 (문자열 간격 맞추기 위해)
        void StringSpace(int fix, int count)
        {
            for (int i = 0; i < fix - count; i++)
            {
                Console.Write(" ");
            }
        }

        // 아이템 출력
        void ItemPrint(List<Item> items, bool inStore, bool isSelect)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Console.Write("- ");

                // 선택(장착시, 구입시)
                if (isSelect)
                {
                    Console.Write($"{i + 1} ");
                }

                // 장착 여부
                if (!inStore)
                {
                    if (items[i].mIsEquipment)
                    {
                        Console.Write("[E]");
                    }
                }

                // 아이템 이름
                Console.Write($"{items[i].mName}");

                // 문자열 간격 맞추기
                if (!inStore)
                {
                    if (items[i].mIsEquipment)
                    {
                        // "[E]"가 3칸
                        StringSpace((int)StringLength.ItemName - 3, StringCount(items[i].mName));
                    }
                    else
                    {
                        StringSpace((int)StringLength.ItemName, StringCount(items[i].mName));
                    }
                }
                else
                {
                    StringSpace((int)StringLength.ItemName, StringCount(items[i].mName));
                }

                // 아이템 공격력
                if (items[i].mPower != 0)
                {
                    Console.Write($" | 공격력 +{items[i].mPower}\t");
                }

                // 아이템 방어력
                if (items[i].mDefense != 0)
                {
                    Console.Write($" | 방어력 +{items[i].mDefense}\t");
                }

                // 아이템 정보
                Console.Write($" | {items[i].mInfo}");
                // 문자열 간격 맞추기
                StringSpace((int)StringLength.ItemInfo, StringCount(items[i].mInfo));

                // 구매여부
                if (inStore)
                {
                    if (items[i].mIsBuy)
                    {
                        Console.Write($" | 구매완료");
                    }
                    else
                    {
                        Console.Write($" | {items[i].mPrice} G");
                    }
                }

                Console.WriteLine();
            }
        }

        void InventoryMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                // 보유한 아이템 리스트
                ItemPrint(mPlayerItems, false, false);
                Console.WriteLine();
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                mIsNum = int.TryParse(Console.ReadLine(), out mSelect);
                if (mIsNum)
                {
                    switch (mSelect)
                    {
                        case 0:
                            return;
                        case 1:
                            // 장착 관리
                            Equipment();
                            continue;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                Console.ReadLine();
            }
        }

        void Equipment()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                // 보유한 아이템 리스트
                ItemPrint(mPlayerItems, false, true);
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                mIsNum = int.TryParse(Console.ReadLine(), out mSelect);
                if (mIsNum)
                {
                    if (mSelect == 0)
                    {
                        return;
                    }
                    // 원하는 행동 (1 ~ 플레이어 아이템 총 개수)
                    else if (mSelect > 0 && mSelect <= mPlayerItems.Count)
                    {
                        // 아이템 해제
                        if (mPlayerItems[mSelect - 1].mIsEquipment)
                        {
                            mPlayerItems[mSelect - 1].mIsEquipment = false;
                            mPlayer.mItemPower -= mPlayerItems[mSelect - 1].mPower;
                            mPlayer.mItemDefense -= mPlayerItems[mSelect - 1].mDefense;
                            continue;
                        }
                        // 아이템 장착
                        else
                        {
                            mPlayerItems[mSelect - 1].mIsEquipment = true;
                            mPlayer.mItemPower += mPlayerItems[mSelect - 1].mPower;
                            mPlayer.mItemDefense += mPlayerItems[mSelect - 1].mDefense;
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                Console.ReadLine();
            }
        }

        void StoreMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{mPlayer.mGold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                // 상점 아이템 리스트
                ItemPrint(mItems, true, false);
                Console.WriteLine();
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                mIsNum = int.TryParse(Console.ReadLine(), out mSelect);
                if (mIsNum)
                {
                    switch (mSelect)
                    {
                        case 0:
                            return;
                        case 1:
                            // 아이템 구매
                            BuyItems();
                            continue;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");

                }
                Console.ReadLine();
            }
        }

        void BuyItems()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{mPlayer.mGold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                // 상점 아이템 리스트
                ItemPrint(mItems, true, true);
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                mIsNum = int.TryParse(Console.ReadLine(), out mSelect);
                if (mIsNum)
                {
                    if (mSelect == 0)
                    {
                        return;
                    }
                    // 원하는 행동 (1 ~ 상점 아이템 총 개수)
                    else if (mSelect > 0 && mSelect <= mItems.Count)
                    {
                        if (mItems[mSelect - 1].mIsBuy == true)
                        {
                            Console.WriteLine("이미 구매한 아이템입니다.");
                        }
                        else
                        {
                            if (mItems[mSelect - 1].mPrice > mPlayer.mGold)
                            {
                                Console.WriteLine("Gold 가 부족합니다.");
                            }
                            // 구매 완료
                            else
                            {
                                mPlayer.mGold -= mItems[mSelect - 1].mPrice;
                                mItems[mSelect - 1].mIsBuy = true;
                                mPlayerItems.Add(mItems[mSelect - 1]);
                                Console.WriteLine("구매를 완료했습니다.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                Console.ReadLine();
            }
        }
    }

    class Player
    {
        // 플레이어 레벨
        public int mLevel { get; set; }
        // 플레이어 이름
        public string mName { get; set; }
        // 플레이어 직업
        public string mClass { get; set; }
        // 플레이어 공격력
        public int mPower { get; set; }
        // 플레이어 아이템 공격력
        public int mItemPower { get; set; }
        // 플레이어 방어력
        public int mDefense { get; set; }
        // 플레이어 아이템 방어력
        public int mItemDefense { get; set; }
        // 플레이어 체력
        public int mHp { get; set; }
        // 플레이어 아이템 체력
        public int mItemHp { get; set; }
        // 플레이어 소지 골드
        public int mGold { get; set; }


        public Player(int playerLevel, string playerName, string playerClass, int playerPower, int playerDefense, int playerHp, int playerGold)
        {
            mLevel = playerLevel;
            mName = playerName;
            mClass = playerClass;
            mPower = playerPower;
            mDefense = playerDefense;
            mHp = playerHp;
            // 처음 생성된 플레이어는 아이템 능력치 0 (아무것도 장착 X인 상태)
            mItemPower = 0;
            mItemDefense = 0;
            mItemHp = 0;
            mGold = playerGold;
        }
    }

    class Item
    {
        // 아이템 이름
        public string mName { get; set; }
        // 아이템 공격력
        public int mPower { get; set; }
        // 아이템 방어력
        public int mDefense { get; set; }
        // 아이템 정보
        public string mInfo { get; set; }
        // 아이템 장착 여부
        public bool mIsEquipment { get; set; }
        // 아이템 가격
        public int mPrice { get; set; }
        // 아이템 구매 여부
        public bool mIsBuy { get; set; }

        public Item(string name, int power, int defense, string info, int price)
        {
            mName = name;
            mPower = power;
            mDefense = defense;
            mInfo = info;
            mPrice = price;
            // 처음 생성된 아이템은 장착 여부, 구매 여부 X인 상태
            mIsEquipment = false;
            mIsBuy = false;
        }
    }
}
