export const deviceType = (): string => {

    const userAgent = navigator.userAgent;
    if (/Android/i.test(userAgent)) {
      return "Mobile";
    } else if (/iPhone|iPad|iPod/i.test(userAgent)) {
      return "Tablet";
    } else if (/Windows/i.test(userAgent)) {
      return "Pc";
    } else {
      return "Unknown";
    }
  };

