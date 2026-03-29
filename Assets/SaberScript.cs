using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portals
{
    public class SaberScript : MonoBehaviour
    {
        public PortalScript PortalManager;
        public GameObject[] PortalCosmetics;
        public GameObject Portal;

        private void OnTriggerEnter(Collider other)
        {
            if (PortalCosmetics != null)
            {
                foreach (GameObject objy in PortalCosmetics)
                {
                    if (objy != null) objy.SetActive(true);
                }
            }

            if (Portal != null)
            {
                Portal.SetActive(false);
            }

            if (PortalManager != null)
            {
                PortalManager.RestartTimer();
            }
        }
    }
}