using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ChangeSpecController : MonoBehaviour
{
    [Serializable]
    public struct StoneTypeSelector
    {
        public StoneType type;
        public SpriteRenderer renderer;
    }
    
    public bool active;
    public SkillTable skillTable;
    public StoneDisplayConfig stoneConfig;
    public Inventory inventory;
    public Camera cam;
    
    
    public float displayDepth;
    public float hideDepth;
    
    public Vector2 wandOffset;
    
    // use this range and each stone's (position, buttonPositon) to compute a reaction range.
    public float actionRadius;
    
    public SpriteRenderer disableTag;
    public SpriteRenderer insertTag;
    
    [Header("Texts")]
    public TextMesh textCurMagicMax;
    public TextMesh textCurMagicRec;
    
    public TextMesh textStoneMagicMax;
    public TextMesh textStoneMagicRec;
    
    public TextMesh spiritCount;
    public TextMesh powerCount;
    public TextMesh natureCount;
    public TextMesh indicatorCount;
    
    public TextMesh[] magicChangeLabels;
    public string numberFormat;
    
    [Header("Selection")]
    public float typeSelectRadius;
    public StoneTypeSelector[] selectors;
    public SpriteRenderer selectTypeTag;
    
    public SpriteRenderer selectLeft;
    public SpriteRenderer selectRight;
    public RectTransform selectLeftArea;
    public RectTransform selectRightArea;
    public Color arrowActiveColor;
    public Color arrowDeactiveColor;
    
    public bool leftSelecting;
    public bool rightSelecting;
    
    public SpriteRenderer leftStone;
    public SpriteRenderer centerStone;
    public SpriteRenderer rightStone;
    
    public Vector2 stoneSideScale;
    public Vector2 stoneCenterScale;
    
    [Header("Visualize & UI")]
    public float scaleSize;
    public float curScale => Camera.main.orthographicSize * scaleSize;
    
    [Header("Selection info")]
    // Which stone type is player's cursor hovering?
    public StoneType curSelectingType;
    // Which type of stones are certainly selected to be shown?
    public StoneType curSelectionType;
    
    public StoneSlot curSelectSlot;
    
    readonly Dictionary<StoneType, int> curSelectStoneIndex = new Dictionary<StoneType, int>();
    List<Stone> selectedStones;
    Stone selectedStone =>
        curSelectStoneIndex[curSelectionType] < 0 || curSelectStoneIndex[curSelectionType] >= selectedStones.Count ?
        null : selectedStones[curSelectStoneIndex[curSelectionType]];
    
    void Update()
    {
        // Init if not.
        foreach(var i in Enum.GetValues(typeof(StoneType)))
        {
            var type = (StoneType)i;
            if(!curSelectStoneIndex.ContainsKey(type)) curSelectStoneIndex.Add(type, 0);
        }
        
        // Information update.
        UpdateInventory();
        UpdateCurSelectStoneSlot();
        UpdateTypeSelecting();
        
        // UI update.
        UpdateActivation();
        UpdateSelfPosition();
        UpdateWandPosition();
        UpdateDisableTags();
        UpdateTypeSelectors();
        UpdateSelectionArrow();
        UpdateStoneSelection();
        UpdateScale();
        UpdateText();
        
        if(!active) return;
        
        // Actions.
        SelectType();
        SelectStone();
        if(!DestroyStone()) UseStone();
    }
    
    void UpdateInventory()
    {
        selectedStones = inventory.UnusedStoneOfType(curSelectionType);
        selectedStones.Sort((x, y) =>
            x.maxMagic == y.maxMagic ? (
                x.magicRecoverRate < y.magicRecoverRate ? -1 :
                x.magicRecoverRate > y.magicRecoverRate ? 1 :
                0
            ) :
            x.maxMagic < y.maxMagic ? -1 : 1
        );
    }
    
    
    void UpdateCurSelectStoneSlot()
    {
        curSelectSlot = null;
        foreach(var st in inventory.curWand.stoneSlots)
        {
            var dist = ((Vector2)st.transform.position).To(Util.cursorWorldPosition).magnitude / curScale;
            if(dist <= actionRadius)
            {
                curSelectSlot = st;
                break;
            }
        }
    }
    
    void UpdateTypeSelecting()
    {
        curSelectingType = StoneType.None;
        foreach(var s in selectors)
        {
            var dist = ((Vector2)s.renderer.transform.position).To(Util.cursorWorldPosition).magnitude / curScale;
            if(dist <= typeSelectRadius)
            {
                curSelectingType = s.type;
                break;
            }
        }
    }
    
    void UpdateActivation()
    {
        if(active)
        {
            inventory.curWand.display.active = true;
            this.ForeachChild(Util.Activate);
        }
        else
        {
            inventory.curWand.display.active = false;
            this.ForeachChild(Util.Deactive);
        }
    } 
    
    void UpdateSelfPosition()
    {
        this.transform.position = cam.transform.position;
    }
    
    void UpdateWandPosition()
    {
        // Synchronize position no matter if activated.
        inventory.curWand.transform.position = (Vector2)cam.transform.position + wandOffset * curScale;
        
        if(active) inventory.curWand.transform.position = inventory.curWand.transform.position.Z(displayDepth);
        else inventory.curWand.transform.position = inventory.curWand.transform.position.Z(hideDepth);
    }
    
    void UpdateDisableTags()
    {
        if(curSelectSlot == null)
        {
            disableTag.color = disableTag.color.A(0f);
            insertTag.color = insertTag.color.A(0f);
            return;
        }
        
        if(curSelectSlot.stone == null)
        {
            insertTag.color = insertTag.color.A(1f);
            insertTag.transform.position = curSelectSlot.transform.position;
            return;
        }
        
        disableTag.color = disableTag.color.A(1f);
        disableTag.transform.position = curSelectSlot.transform.position;
    }
    
    void UpdateTypeSelectors()
    {
        // Current mouse hovering type.
        // Todo with curSelectingType.
        if(curSelectingType == StoneType.None)
        {
            selectTypeTag.color = selectTypeTag.color.A(0f);
        }
        else
        {
            selectTypeTag.color = selectTypeTag.color.A(1f);
            foreach(var s in selectors) if(s.type == curSelectingType) selectTypeTag.transform.position = s.renderer.transform.position;
        }
        
        // Selected stone type is also change the effect...
        // Todi with curSelectionType.
        foreach(var s in selectors)
        {
            s.renderer.sprite = stoneConfig.GetSprite(s.type);
            s.renderer.color = stoneConfig.GetColor(s.type);
            if(curSelectionType == s.type) s.renderer.color = s.renderer.color.A(1f);
            else s.renderer.color = s.renderer.color.A(0.5f);
        }
        
        
    }
    
    void UpdateSelectionArrow()
    {
        bool SetArrowColor(SpriteRenderer rd, Rect area, Vector2 pos, Vector2 scale, Color active, Color deactive)
        {
            if(area.Contains((Util.cursorWorldPosition - pos) / scale / curScale))
            {
                rd.color = arrowActiveColor;
                return true;
            }
            
            rd.color = arrowDeactiveColor;
            return false;
        }
        
        leftSelecting = SetArrowColor(selectLeft, selectLeftArea.rect, selectLeftArea.position, selectLeftArea.localScale, arrowActiveColor, arrowDeactiveColor);
        rightSelecting = SetArrowColor(selectRight, selectRightArea.rect, selectRightArea.position, selectLeftArea.localScale, arrowActiveColor, arrowDeactiveColor);
    }
    
    void UpdateStoneSelection()
    {
        int id = curSelectStoneIndex[curSelectionType];
        Stone l = id - 1 < 0 ? null : selectedStones[id - 1];
        Stone c = selectedStone;
        Stone r = id + 1 >= selectedStones.Count ? null : selectedStones[id + 1];
        
        void SetStone(SpriteRenderer rd, Stone x, Vector2 scale)
        {
            if(x == null)
            {
                rd.color = rd.color.A(0f);
                return;
            }
            
            rd.sprite = stoneConfig.GetSprite(x.type);
            rd.color = stoneConfig.GetColor(x.type);
            rd.transform.localScale = scale;
        }
        
        SetStone(leftStone, l, stoneSideScale);
        SetStone(centerStone, c, stoneCenterScale);
        SetStone(rightStone, r, stoneSideScale);
    }
    
    void UpdateScale()
    {
        inventory.curWand.transform.localScale = curScale * Vector2.one;
        this.transform.localScale = curScale * Vector3.one;
    }
    
    void UpdateText()
    {
        if(curSelectSlot != null && curSelectSlot.stone != null)
        {
            textCurMagicMax.text = curSelectSlot.stone.maxMagic.ToString(numberFormat);
            textCurMagicRec.text = curSelectSlot.stone.magicRecoverRate.ToString(numberFormat);
        }
        
        if(selectedStone == null)
        {
            textStoneMagicMax.text = "";
            textStoneMagicRec.text = "";
            foreach(var t in magicChangeLabels) t.color = t.color.A(0f);
        }
        else
        {
            textStoneMagicMax.text = selectedStone.maxMagic.ToString(numberFormat);
            textStoneMagicRec.text = selectedStone.magicRecoverRate.ToString(numberFormat);
            foreach(var t in magicChangeLabels) t.color = t.color.A(1f);
        }
        
        var cnt = inventory.CountAllStone();
        spiritCount.text = cnt[StoneType.Spirit].ToString();
        powerCount.text = cnt[StoneType.Power].ToString();
        natureCount.text = cnt[StoneType.Nature].ToString();
        indicatorCount.text = cnt[StoneType.Indicator].ToString();
    }
    
    void SelectType()
    {
        if(curSelectingType != StoneType.None && Input.GetKeyDown(KeyCode.Mouse0))
        {
            curSelectionType = curSelectingType;
        }
    }
    
    void SelectStone()
    {
        if(curSelectionType == StoneType.None) return;
        
        var curStones = inventory.UnusedStoneOfType(curSelectionType);
        
        // If there's no stone, do nothing.
        if(curStones.Count == 0) return;
        
        // React to the select arrow.
        if(leftSelecting && Input.GetKeyDown(KeyCode.Mouse0))
        {
            curSelectStoneIndex[curSelectionType] -= 1;
        }
        
        if(rightSelecting && Input.GetKeyDown(KeyCode.Mouse0))
        {
            curSelectStoneIndex[curSelectionType] += 1;
        }
        
        // If selection is out of range, select the closest.
        if(curSelectStoneIndex[curSelectionType] < 0) curSelectStoneIndex[curSelectionType] = 0;
        if(curSelectStoneIndex[curSelectionType] >= curStones.Count) curSelectStoneIndex[curSelectionType] = curStones.Count - 1;
    }
    
    
    bool DestroyStone()
    {
        if(!Input.GetKeyDown(KeyCode.Mouse0)) return false;
        if(curSelectSlot == null || curSelectSlot.stone == null) return false;
        Destroy(curSelectSlot.stone.gameObject);
        curSelectSlot.stone = null;
        return true;
    }
    
    bool UseStone()
    {
        if(!Input.GetKeyDown(KeyCode.Mouse0)) return false;
        if(curSelectSlot == null || curSelectSlot.stone != null) return false;
        if(selectedStone != null) curSelectSlot.stone = selectedStone;
        return true;
    }
    void OnDrawGizmosSelected()
    {
        if(!active) return;
        
        Gizmos.color = Color.green;
        foreach(var st in inventory.curWand.stoneSlots) Gizmos.DrawWireSphere(st.transform.position, actionRadius);
        
        Gizmos.color = Color.yellow;
        foreach(var s in selectors) Gizmos.DrawWireSphere(s.renderer.transform.position, typeSelectRadius);
    }
    
}
