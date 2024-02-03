
window.dashPlayers = {};

export function initializeDashPlayer(elementId, videoUrl, hash, autoLoad, playerOptions, dotnet) {
    const player = dashjs.MediaPlayer().create();
    dashPlayers[elementId] = player; // Store the Dash player instance for cleanup
    player.extend("RequestModifier", function () {
        return {
            modifyRequestHeader: function (xhr, { url }) {
                /* Add custom header. Requires to set up Access-Control-Allow-Headers in your */
                /* response header in the server side. Reference: https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest/setRequestHeader */
                /* xhr.setRequestHeader('DASH-CUSTOM-HEADER', 'MyValue'); */
                xhr.setRequestHeader("hash", hash);
                return xhr;
            }
        };
    });

    ///* restart playback in muted mode when auto playback was not allowed by the browser */
    //player.on(dashjs.MediaPlayer.events.PLAYBACK_NOT_ALLOWED, function (data) {
    //    console.log('Playback did not start due to auto play restrictions. Muting audio and reloading');
    //    const video = document.getElementById(elementId);
    //    video.muted = true;
    //    player.initialize(video, videoUrl, autoLoad);
    //});

    // playback ended
    player.on(dashjs.MediaPlayer.events.PLAYBACK_ENDED, function () {
        console.log("Video playback ended");
        dotnet.invokeMethodAsync("OnPlaybackEndedHandler");
    });

    player.initialize();

    player.updateSettings({
        'streaming': {
            'scheduling': {
                'scheduleWhilePaused': playerOptions.scheduleWhilePaused,   /* stops the player from loading segments while paused */
            },
            'buffer': {
                'fastSwitchEnabled': playerOptions.fastSwitchEnabled   /* enables buffer replacement when switching bitrates for faster switching */
            }
        }
    });
    player.setAutoPlay(autoLoad);
    player.attachView(document.getElementById(elementId));

    player.attachSource(videoUrl);

    var controlbar = new ControlBar(player); //Player is instance of Dash.js MediaPlayer;
    controlbar.initialize();

    console.log({ player, controlbar })
    return { player, controlbar };
}

export function changeSource(instance, url) {
    var player = instance.player;
    player.attachSource(url);
    player.setAutoPlay(true);
    player.play();
}

export function showControlBar(instance) {
    instance.controlbar.show();
}

export function hideControlBar(instance) {
    instance.controlbar.hide();
}


export function play(instance) {
    instance.player.play();
}

export function pause(instance) {
    instance.player.pause();
}
