export class Enumerate {
    static shallow(object, when, then) {
        for (let property in object) {
            if (when(property, object[property])) {
                then(property, object[property]);
            }
        }
    }

    static deep(object, when, then) {
        this.deepLoop(object, when, then);
    }

    private static deepLoop(object, when, then) {
        for (let property in object) {
            let type = typeof object[property]

            if (type !== "object") {
                if (when(property, object[property]))
                    then(property, object[property]);
            }
            else {
                this.deepLoop(object[property], when, then);
            }
        }
    }
}