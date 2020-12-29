import * as React from "react";

export default function HeaderBanner(): JSX.Element {
    return (
        /*
         * svelte original
<header>
  <h1>{atem.state._pin}</h1>
  <a href="#switcher" class="tab"><Feather icon="grid"/>Switcher</a>
  <a href="#media" class="tab"><Feather icon="film"/>Media</a>
  <a href="#macros" class="tab"><Feather icon="box"/>Macros</a>
  <span class="tab connection-status" class:connected={ws.readyState === 1}
        title="Connection status: green=connected, red=disconnected">
    {#if ws.readyState === 1}<Feather icon="zap"/>{:else}<Feather icon="alert-triangle"/>{/if}
    Server
  </span>
  <span class="tab connection-status" class:connected={atem.connected}
        title="Connection status: green=connected, red=disconnected">
    {#if atem.connected}<Feather icon="zap"/>{:else}<Feather icon="alert-triangle"/>{/if}
    ATEM
  </span>
</header>
         */
        <header>
            <h1>Switcher Name</h1>
            <a href="#switcher" className="tab">Switcher</a>
            <a href="#media" className="tab">Media</a>
            <a href="#macros" className="tab">Macros</a>
            <span className="tab connection-status"
                title="Connection status: green=connected, red=disconnected">
                Server
            </span>
            <span className="tab connection-status"
                title="Connection status: green=connected, red=disconnected">
                ATEM
            </span>
        </header>
    )
}
