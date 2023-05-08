import asyncio

import carb
import omni.usd
import omni.kit.app

MAX_WAITING_FRAMES = 50

_new_stage_created = False
_app_ready = False

def _on_stage(stage_event):
    if stage_event.type == int(omni.usd.StageEventType.OPENED):
        if omni.usd.get_context().is_new_stage():
            global _new_stage_created
            _new_stage_created = True
_stage_sub = omni.usd.get_context().get_stage_event_stream().create_subscription_to_pop(_on_stage, name="connector quick launch")

def _on_app_ready(event):
    global _app_ready
    _app_ready = True
_app_ready_sub = (
    omni.kit.app.get_app()
    .get_startup_event_stream()
    .create_subscription_to_pop_by_type(
        omni.kit.app.EVENT_APP_READY, _on_app_ready, name="connector app-ready sub"
    )
)

async def _delay_load_stage(stage_path):
    for _ in range(MAX_WAITING_FRAMES):
        if _app_ready and _new_stage_created:
            global _stage_sub
            global _app_ready_sub
            _stage_sub = None
            _app_ready_sub = None  
            break
        await omni.kit.app.get_app().next_update_async()

    await omni.usd.get_context().open_stage_async(stage_path)

stage_path = carb.settings.get_settings().get("/app/stage_path")
asyncio.ensure_future(_delay_load_stage(stage_path))
