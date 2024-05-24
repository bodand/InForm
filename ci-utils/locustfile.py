import time
import random
from locust import FastHttpUser, task, tag, between

test_form_ids = []

setup_data = {
               "title": "string",
               "subtitle": "string",
               "elements": [
                 {
                   "$t": "string",
                   "title": "string0",
                   "subtitle": "",
                   "required": True
                 },
                 {
                   "$t": "string",
                   "title": "string1"
                 },
                 {
                   "$t": "string",
                   "title": "string2",
                   "required": True
                 },
                 {
                   "$t": "mc",
                   "title": "malti choice0",
                   "options": [
                     "asdfasdfasdf",
                     "asdfasdf",
                     "a",
                     "b",
                     "c"
                   ],
                   "selectable": 2
                 },
                 {
                   "$t": "mc",
                   "title": "malti choice1",
                   "options": ["0", "1", "2", "3", "4"],
                   "selectable": 1
                 }
               ]
             }

def fill(id, elem):
    return {
        "formId": id,
        "elements": [
            { "$t": "mc", "selected": ["asdfasdfasdf", "asdfasdf"], "id": elem[0] },
            { "$t": "mc", "selected": ["4"], "id": elem[1] },
            { "$t": "string", "value": "dfsh", "id": elem[2] },
            { "$t": "string", "value": "asDasd", "id": elem[3] },
            { "$t": "string", "value": "asdfasdfasdfasdf", "id": elem[4] }
        ]
    }

class FormFillerUser(FastHttpUser):
    wait_time = between(1, 5)

    def on_start(self):
        with self.rest('POST', f'/api/forms', json=setup_data) as resp:
            if resp.js is not None:
                new_id = resp.js["id"]
                test_form_ids.append(new_id)

    @task(20)
    def fill_form(self):
        form_id = random.choice(test_form_ids)
        elem_id = []
        with self.rest("GET", f"/api/forms/{form_id}", name="/api/forms/[id]") as resp:
            if resp.js is None:
                return
            elif "id" not in resp.js:
                resp.failure("missing id in response")
                return
            elif resp.js["id"] != form_id:
                resp.failure("mismatched id in response: {id}" % resp.js["id"])
                return

            for e in resp.js["formElements"]:
                elem_id.append(e["id"])

        with self.rest("POST", f"/api/fills/{form_id}", name="/api/fills/[id]", json=fill(form_id, elem_id)) as resp:
            if resp.js is None:
                pass
            elif "id" not in resp.js:
                resp.failure("missing id in response")
            elif resp.js["id"] != form_id:
                resp.failure("mismatched id in response: {id}" % resp.js["id"])

    @task(1)
    def read_responses(self):
        form_id = random.choice(test_form_ids)
        with self.rest("POST", f"/api/fills/{form_id}/:retrieve", name="/api/fills/[id]:retrieve", json={"id": form_id}) as resp:
            if resp.js is None:
                pass
