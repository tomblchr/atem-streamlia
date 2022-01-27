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

    if (host) {
        return fetch(`https://${host}/api/atemconnection/streamliaurl`, options);
        //return fetch(`https://${host}/echo/get/json`, options);
    }

    return fetch("/api/atemconnection/streamliaurl", options);
};
