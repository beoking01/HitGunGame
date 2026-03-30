using UnityEngine;
using UnityEngine.SceneManagement;

public class TruckInventory : MonoBehaviour
{
    [Header("Cài đặt thùng xe")]
    public string itemTag = "Item";

    // Đang bị lỗi vật phẩm không ra được khi trở basecentral, 
    // nên sẽ tách riêng hàm ReleaseItem để đảm bảo nó luôn được gọi khi cần thiết, không phụ thuộc vào sự kiện OnTriggerExit có thể bị lỗi.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(itemTag))
        {
            other.transform.SetParent(this.transform);

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Khi ở trong xe, nên tắt vật lý để tránh va chạm lỗi (Jitter)
                rb.isKinematic = true; 
                rb.useGravity = false;
            }
            Debug.Log("Đã gắn " + other.name + " vào xe tải!");
        }
    }

    // HÀM QUAN TRỌNG: Gọi hàm này khi Player "ném" đồ ra ngoài
    public void ReleaseItem(GameObject item)
    {
        // 1. Tách khỏi xe ngay lập tức (về Root)
        item.transform.SetParent(null);

        // 2. Đưa vật phẩm vào Scene hiện hành (Ví dụ: BaseCentral)
        // Phải kiểm tra xem vật phẩm có đang ở trong Scene DontDestroyOnLoad không
        Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (item.scene != activeScene)
        {
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(item, activeScene);
        }

        // 3. Trả lại trạng thái vật lý
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            
            // Mẹo: Thêm một lực đẩy nhẹ để vật phẩm không bị kẹt vào thành xe khi vừa thoát ra
            rb.AddForce(transform.up * 2f, ForceMode.Impulse);
        }

        Debug.Log($"[Truck] Đã giải phóng {item.name} về Scene: {activeScene.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(itemTag))
        {
            // Kiểm tra xem nó có đang thực sự là con của xe này không trước khi tách
            if(other.transform.parent == this.transform)
            {
                ReleaseItem(other.gameObject);
            }
        }
    }
}
