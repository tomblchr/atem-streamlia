import * as Log from './log';

export async function apiTransition(): Promise<Response> {
    const options: RequestInit = {
        method: "POST"
    };
    return fetch("/api/atemconnection/transition", options);
};

export async function hostURL(host?: string): Promise<Response> {
    const options: RequestInit = {
        method: "GET"
    };

    const url = `https://${host}/api/atemconnection/streamliaurl`;

    if (host) {
        Log.debug(`Connecting to server @ ${url}`);
        return fetch(url, options);
    }

    return fetch("/api/atemconnection/streamliaurl", options);
};
