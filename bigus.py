from locust import HttpUser, task, between

class ExamNotificationUser(HttpUser):
    # Simulate a wait time between tasks (in seconds)
    wait_time = between(1, 2)

    @task
    def fetch_exam_notifications(self):
        url = ""
        with self.client.get(url, verify=False, catch_response=True) as response:
            if response.status_code == 200:
                response.success()
            else:
                response.failure(f"Failed with status code {response.status_code}")
