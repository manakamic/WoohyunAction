﻿using UnityEngine;
            v -= 0.1f;
        }
            v += 0.1f;
        }
            h -= 1.0f;
        }
            h += 1.0f;
        }
        on_rotate_left_ = true;
        on_rotate_right_ = false; // Cancel.
    }
        on_rotate_left_ = false;
    }
        on_rotate_right_ = true;
        on_rotate_left_ = false; // Cancel.
    }
        on_rotate_right_ = false;
    }
        on_zoom_up_ = true;
        on_zoom_down_ = false; // Cancel.
    }
        on_zoom_up_ = false;
    }
        on_zoom_down_ = true;
        on_zoom_up_ = false; // Cancel.
    }
        on_zoom_down_ = false;
    }