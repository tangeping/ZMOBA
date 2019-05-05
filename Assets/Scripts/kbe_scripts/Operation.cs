namespace KBEngine
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Operation : OperationBase
    {
        public Operation() : base()
        {

        }

        public override void onAttached(Entity ownerEntity)
        {
            if (ownerEntity.isPlayer())
            {
                KBEngine.Event.registerIn("reqReady", this, "reqReady");
                KBEngine.Event.registerIn("reqStop", this, "reqStop");
                KBEngine.Event.registerIn("reqRun", this, "reqRun");

                KBEngine.Event.registerIn("reqHeroList", this, "reqHeroList");
                KBEngine.Event.registerIn("reqSelectHero", this, "reqSelectHero");
            }
        }

        public override  void onEnterworld()
        {
            if (this.owner.isPlayer())
            {
                Debug.Log("---onCellReady---");
                this.reqHeroList();
                KBEngine.Event.fireOut("onChooseHeroBegin");

                reqHeroConf();
                reqRoadConf();
                reqPropsConf();
                reqShopConf();
                reqSkillConf();
                reqTeamConf();               
            }
        }

        public override void readyResult(byte result)
        {
            KBEngine.Event.fireOut("readyResult", result);
        }
        public void reqHeroList()
        {
            cellEntityCall.reqHeroList();
            Debug.Log("Operation::reqHeroList," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"));
        }


        public void reqSelectHero(Int32 heroID)
        {
            cellEntityCall.reqSelectHero(heroID);
        }

        public void reqReady(Byte ready)
        {
            cellEntityCall.reqReady(ready);
        }

        public void reqStop()
        {
            cellEntityCall.reqGamePause();
        }

        public void reqRun()
        {
            cellEntityCall.reqGameRunning();
        }

        public void reqHeroConf()
        {
            cellEntityCall.reqHeroConf();
        }

        public void reqRoadConf()
        {
            cellEntityCall.reqRoadConf();
        }

        public void reqPropsConf()
        {
            cellEntityCall.reqPropsConf();
        }

        public void reqShopConf()
        {
            cellEntityCall.reqShopConf();
        }
        public void reqSkillConf()
        {
            cellEntityCall.reqSkillConf();
        }
        public void reqTeamConf()
        {
            cellEntityCall.reqTeamConf();
        }

        public override void broadGameStart()
        {
            KBEngine.Event.fireOut("broadGameStart");
        }

        public override void reqHeroListResult(HERO_BAG heros)
        {
            Debug.Log("Operation::reqHeroListResult,heros.Count = " + heros.Count +","+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"));
            KBEngine.Event.fireOut("reqHeroListResult", heros);
        }

        public override void reqSelectHeroResult(byte result)
        {
            KBEngine.Event.fireOut("reqSelectHeroResult", result);
        }

        public override void rspTeamInfo(D_TEAM_INFOS_LIST teams)
        {
            foreach (var item in teams.values)
            {
                SpaceData.Instance.Teams[item.id] = item;
            }
        }

        public override void rspHeroInfo(D_HERO_INFOS_LIST heros)
        {
            foreach (var item in heros.values)
            {
                SpaceData.Instance.Heros[item.id] = item;
            }
        }

        public override void rspPropsInfo(D_PROPS_INFOS_LIST props)
        {
            foreach (var item in props.values)
            {
                SpaceData.Instance.Props[item.prop_id] = item;
            }
        }

        public override void rspRoadInfo(D_ROAD_INFOS_LIST roads)
        {
            foreach (var item in roads.values)
            {
                if (Enum.IsDefined(typeof(ROAD_TYPE), item.group))
                {
                    var key = (ROAD_TYPE)item.group;
                    if (!SpaceData.Instance.RoadList.ContainsKey(key))
                    {
                        SpaceData.Instance.RoadList[key] = new List<D_ROAD_INFOS>();
                    }
                    SpaceData.Instance.RoadList[key].Add(item);
                }
            }
        }

        public override void rspShopInfo(D_SHOP_INFOS_LIST shops)
        {
            foreach (var item in shops.values)
            {
                SpaceData.Instance.Shops[item.shop_id] = item;
            }
        }

        public override void rspSkillInfo(D_SKILL_INFOS_LIST skills)
        {
            foreach (var item in skills.values)
            {
                SpaceData.Instance.Skills[item.id] = item;
            }
        }



    }
}
