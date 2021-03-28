export async function apiTransition(): Promise<Response> {
    const options: RequestInit = {
        method: "POST"
    };
    return fetch("/api/atemconnection/transition", options);
};

export async function hostURL(): Promise<Response> {
    const options: RequestInit = {
        method: "GET"
    };
    return fetch("/api/atemconnection/streamliaurl", options);
};
