using System;
using System.Collections.Generic;
using Enums;
using Managers;
using Player;
using Store.UI;
using UI;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform startPoint;
    public Transform player;
    public List<ProofPlaceUI> proofUIs;
    public ProofMenuUI proofMenu;
    public Inventory.Inventory inventory;
    public ProofFoundService proofFoundService;
    public PlayerAbilities playerAbilities;

    public StoreUI storeUI;
    public BaseBrainTeaserUI brainteaserUI;

    public string itemId;
    public int amount;

    public Transform tempTestTransform;

    public UIMessageService _messageService;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            player.position = startPoint.position;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            
        }
    }

    public void RandomTape()
    {
        foreach (var proof in proofUIs)
        {
            proof.SetTape();
        }
    }

    public void GiveItem()
    {
        inventory.AddItem(itemId, amount);
    }
    
    public void RandomDrawing()
    {
        proofMenu.SetDrawing();
    }

    public void TestOpenUIWindowsOptimize()
    {
        DateTime start;
        DateTime end;
        
        start = DateTime.Now;
        brainteaserUI.Open();
        end = DateTime.Now;
        Debug.Log("Open brainteaser: " + (end - start).TotalSeconds + " sec");
        
        start = DateTime.Now;
        brainteaserUI.Close();
        end = DateTime.Now;
        Debug.Log("Close brainteaser: " + (end - start).TotalSeconds + " sec");
        
        start = DateTime.Now;
        storeUI.Open();
        end = DateTime.Now;
        Debug.Log("Open store: " + (end - start).TotalSeconds + " sec");
        
        start = DateTime.Now;
        storeUI.Close();
        end = DateTime.Now;
        Debug.Log("Close store: " + (end - start).TotalSeconds + " sec");
    }

    public void TestProofServiceUIOptimize()
    {
        DateTime start;
        DateTime end;
        
        start = DateTime.Now;
        proofMenu.Open();
        end = DateTime.Now;
        Debug.Log("Open proof menu: " + (end - start).TotalSeconds + " sec");
        
        start = DateTime.Now;
        proofMenu.Close();
        end = DateTime.Now;
        Debug.Log("Close proof menu: " + (end - start).TotalSeconds + " sec");
    }
    
    public void TestProofServiceOptimize()
    {
        DateTime start;
        DateTime end;
        start = DateTime.Now;
        proofFoundService.AddProof("watery-paw-prints");
        end = DateTime.Now;
        Debug.Log("add one proof: " + (end - start).TotalSeconds + " sec");
        
        start = DateTime.Now;
        proofFoundService.AddProof("garry-talks");
        proofFoundService.AddProof("origami-frog-talks");
        proofFoundService.AddProof("teddy-bear-talks");
        end = DateTime.Now;
        Debug.Log("add three proofs same type: " + (end - start).TotalSeconds + " sec");
        
        start = DateTime.Now;
        proofFoundService.AddProof("soldier-talks");
        proofFoundService.AddProof("paint-paw-prints");
        proofFoundService.AddProof("karp-talks");
        end = DateTime.Now;
        Debug.Log("add three proofs different types: " + (end - start).TotalSeconds + " sec");
        
        start = DateTime.Now;
        proofFoundService.GetAuthorProofs(author: ProofType.Cat);
        end = DateTime.Now;
        Debug.Log("get Cat proofs: " + (end - start).TotalSeconds + " sec");
        
        start = DateTime.Now;
        proofFoundService.GetPercentageAuthorGuilty(author: ProofType.Cat);
        end = DateTime.Now;
        Debug.Log("get Cat quilty: " + (end - start).TotalSeconds + " sec");
    }
    
    public void TestPlayerAbilsOptimize()
    {
        DateTime start = DateTime.Now;
        playerAbilities.TryAddAbility("school-card");
        DateTime end = DateTime.Now;
        Debug.Log("add ability: " + (end - start).TotalSeconds + " sec");
    }
    
    public void TestInventoryOptimize()
    {
        DateTime start = DateTime.Now;
        inventory.AddItem("red-apple", 16);
        DateTime end = DateTime.Now;
        Debug.Log("add to inventory in 2 slots: " + (end - start).TotalSeconds + " sec");
        inventory.AddItem("juicer", 1);

        start = DateTime.Now;
        inventory.OnItemSelect(1);
        end = DateTime.Now;
        Debug.Log("item select in inventory: " + (end - start).TotalSeconds + " sec");
        
        start = DateTime.Now;
        inventory.OnItemSelect(1);
        inventory.OnItemSelect(4);
        end = DateTime.Now;
        Debug.Log("craft in inventory: " + (end - start).TotalSeconds + " sec");

        start = DateTime.Now;
        inventory.TryRemoveItem("red-apple", 5);
        end = DateTime.Now;
        Debug.Log("remove from inventory: " + (end - start).TotalSeconds + " sec");

    }
    
}